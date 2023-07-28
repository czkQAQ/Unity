using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UniRx;

public class Enemy_Bat : MonoBehaviour,IEnemy
{
    public enum MOVEMODE{AUTO,PATH}
    public MOVEMODE InitMode = MOVEMODE.AUTO;

    public RaycastHit2D  hitColl;
    public float radius, radius2;
    private Vector2 centerPos;
    public LayerMask lmPlayer;
    public LayerMask lmColl;

    private Rigidbody2D rb;
    private GameObject Trigger;
    private GameObject Collider;
    public float speed;
    private bool canMove;
    private Vector2 dir;

    //PATH模式的路点
    private Vector2 targetPos1;
    private Vector2 targetPos2;
    private Vector2 targetPos3;
    private List<Vector2> targetPointList=new List<Vector2>();
    public bool stickMove;
    public GameObject stick;

    public SkeletonAnimation saPlayer;
    public enum EnemyDir { NONE, LEFT, RIGHT }
    [SerializeField] EnemyDir beginDir = EnemyDir.NONE;
    [SpineAnimation]
    public string str_idle, str_Attack, str_Die, str_run;

    Collider2D hitPlayer;
    Collider2D hitAtt;
    private bool isAttack;

    public ParticleSystem pBlood;
    public ParticleSystem pSkull;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        if (MapLevelManager.Instance != null)
        {
            MapLevelManager.Instance.lstAllEnemies.Add(this);
        }

        canMove = true;  

        rb = transform.GetComponent<Rigidbody2D>();
        Collider = transform.Find("Collider").gameObject;
        Trigger = transform.Find("Trigger").gameObject;

        if(InitMode == MOVEMODE.PATH)
        {
            targetPos1 = transform.Find("TargetPos1").position;
            targetPos2 = transform.Find("TargetPos2").position;
            targetPos3 = transform.Find("TargetPos3").position;
            targetPointList.Add(targetPos1);
            targetPointList.Add(targetPos2);
            targetPointList.Add(targetPos3);
        }

        if (beginDir == EnemyDir.LEFT || beginDir == EnemyDir.NONE)
        {
            saPlayer.skeleton.ScaleX = 1;
        }
        else if (beginDir == EnemyDir.RIGHT)
        {
            saPlayer.skeleton.ScaleX = -1;
        }

        saPlayer.AnimationState.Start += Event;
    }

    private bool player, enemy, hostage;
    private void Event(TrackEntry trackEntry)
    {
        if (saPlayer.AnimationName.Equals(str_Attack))
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.acMeleeAttack);
            }

            var time = saPlayer.skeleton.Data.FindAnimation(str_Attack).Duration;
            Observable.Timer(System.TimeSpan.FromSeconds(time * 0.55f)).Subscribe(_ =>
            {
                if (player)
                {
                    
                        PlayerManager.Instance.OnPlayerDie(true);
                        GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                   
                }
                if (hostage)
                {
                    if (hitPlayer.gameObject.tag == "Hostage")
                    {
                        hitAtt.gameObject.GetComponent<HostageManager>().OnDie_(true);
                        hitAtt.gameObject.GetComponent<HostageManager>().PlayDie();
                        GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                    }
                }
                if (enemy)
                {
                    if (hitAtt.gameObject.layer == 15)
                    {
                        hitAtt.gameObject.GetComponentInParent<EnemyMelee>()?.OnDie_();
                        hitAtt.gameObject.GetComponentInParent<EnemyRange>()?.OnDie_();
                    }
                }
            });
        }

    }

    private void Update()
    {
        centerPos = transform.position;

        if (InitMode == MOVEMODE.AUTO)
        {
            hitPlayer = Physics2D.OverlapCircle(centerPos, radius, lmPlayer);
            if (hitPlayer != null)
            {
                if (die) return;
                hitColl = Physics2D.Linecast(centerPos, hitPlayer.gameObject.transform.position, lmColl);
                if (!hitColl.collider && !isAttack)
                {
                    dir = new Vector2(hitPlayer.gameObject.transform.position.x - centerPos.x, hitPlayer.gameObject.transform.position.y - centerPos.y);
                    
                    if(hitPlayer.gameObject.transform.position.x - centerPos.x > 0)
                    {
                        saPlayer.skeleton.ScaleX = -1;
                    }
                    else
                    {
                        saPlayer.skeleton.ScaleX = 1;
                    }

                    Move(dir);
                    PlayAnim(str_run, true);
                }
            }
        }

        if(stick!=null&& stick.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            stickMove = true;
        }
        if (stick == null) stickMove = true;
            if (InitMode == MOVEMODE.PATH)
        {
            if (!stickMove) return;
            //MoveToTarget(targetPos1, targetPos2, targetPos3);
            MoveToTarget();
        }

        if (!canMove)
        {
            rb.velocity = new Vector2(0, 0);
        }

        hitAtt = Physics2D.OverlapCircle(centerPos, radius2, lmPlayer);
        if(hitAtt.gameObject.tag == "BodyPlayer")
        {
            if (!PlayerManager.Instance.isTakeSword)
            {
                canMove = false;
                isAttack = true;
                player = true;
                PlayAnim(str_Attack, false);
                StartCoroutine(Return2Idle());
            }
            else if(PlayerManager.Instance.isTakeSword && !die)
            {
                canMove = false;
                isAttack = true;
                PlayAnim(str_idle, true);
                PlayerManager.Instance.PlayAnim(PlayerManager.Instance.str_Att, false);
                Invoke("OnDie", 0.7f);
            }
            
        }
        if(hitAtt.gameObject.tag == "Hostage")
        {
            canMove = false;
            isAttack = true;
            hostage = true;
            PlayAnim(str_Attack, false);
            StartCoroutine(Return2Idle());
        }
        if(hitAtt.gameObject.layer == 15)
        {
            canMove = false;
            isAttack = true;
            enemy = true;
            PlayAnim(str_Attack, false);
            StartCoroutine(Return2Idle());
        }
    }

    private void PlayAnim(string anim, bool isLoop)
    {
        if (!saPlayer.AnimationName.Equals(anim))
        {
            saPlayer.AnimationState.SetAnimation(0, anim, isLoop);
        }
    }

    private void Move(Vector2 dir)
    {
        if (!canMove) return; 
        rb.velocity = dir.normalized * speed;
    }

    private bool die;
    public void OnDie()
    {
        die = true;
        Trigger.SetActive(false);
        PlayAnim(str_Die, false);
        transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        transform.GetComponent<Rigidbody2D>().gravityScale = 1;
        Collider.layer = 23;
        Collider.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, 0.1f);

        pBlood.Play();
        pSkull.Play();

        AudioManager.Instance.Play(AudioConstant.bat_die);

        MapLevelManager.Instance.lstAllEnemies.Remove(this);
        if (MapLevelManager.Instance.questType == MapLevelManager.QUEST_TYPE.KILL && MapLevelManager.Instance.lstAllEnemies.Count == 0)
        {
            PlayerManager.Instance.OnWin(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag == "BodyPlayer")
        //{
        //    canMove = false;
        //    if (!PlayerManager.Instance.isTakeSword)
        //    {
        //        PlayerManager.Instance.OnPlayerDie(true);
        //        GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
        //    }
        //    else
        //    {
        //        //这里怎么调用主角的攻击函数？
        //        OnDie();
        //    }
        //}
        //if (collision.tag == "Hostage")
        //{
        //    canMove = false;

        //    collision.gameObject.GetComponent<HostageManager>().OnDie_(true);
        //    collision.gameObject.GetComponent<HostageManager>().PlayDie();
        //    GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
        //}
        //if (collision.gameObject.layer == 15)
        //{
        //    collision.GetComponentInParent<EnemyMelee>()?.OnDie_();
        //    collision.GetComponentInParent<EnemyRange>()?.OnDie_();
        //    //collision.GetComponentInParent<EnemyCharged>()?.OnDie_();
        //}

        if (collision.gameObject.tag == Utils.TAG_LAVA || collision.gameObject.tag == "Trap_Lava"
            || collision.gameObject.tag == "Trap_Other" || collision.gameObject.tag == "Trap_Gas" || collision.gameObject.tag == "arrow")
        {
            OnDie();
            if (collision.gameObject.tag == "arrow")
            {
                collision.gameObject.SetActive(false);
                AudioManager.Instance.Play(AudioConstant.dmg_shoot);
            }
        }
        if (collision.gameObject.tag == "fire")
        {
            OnDie();
        }

        if (collision.tag == "Charged" && transform.gameObject.name == "Enemy_Bat")
        {
            OnDie();
            AudioManager.Instance.Play(AudioConstant.dmg_electric);
        }
    }

    //冰冻效果
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Ice")
        {
            OnDie();
            AudioManager.Instance.Play(AudioConstant.changing_ice);
            gameObject.tag = "Tag_Stone";
            gameObject.layer = 10;
        }
    }

    //private bool target1, target2, target3;
    //private void MoveToTarget(Vector2 v1,Vector2 v2,Vector2 v3)
    //{
        

    //    if (!target1)
    //    {
    //        Vector2 dir1 = new Vector2(v1.x - centerPos.x, v1.y - centerPos.y);
    //        if(v1.x - centerPos.x > 0)
    //        {
    //            saPlayer.skeleton.ScaleX = -1;
    //        }
    //        else
    //        {
    //            saPlayer.skeleton.ScaleX = 1;
    //        }
    //        Move(dir1);
    //    }
    //    if (Vector2.Distance(centerPos,v1) < 0.1f&&!target1)
    //    {
    //        target1 = true;       
    //    }
    //    if(target1 && !target2)
    //    {
    //        var dir2 = new Vector2(v2.x - centerPos.x, v2.y - centerPos.y);
    //        saPlayer.skeleton.ScaleX = v2.x - centerPos.x > 0 ? -1 : 1;
    //        Move(dir2);
    //    }
    //    if(Vector2.Distance(centerPos,v2) < 0.1f&&target1&&!target2)
    //    {
    //        target2 = true;
    //    }
    //    if (target2 && !target3)
    //    {
    //        var dir3 = new Vector2(v3.x - centerPos.x, v3.y - centerPos.y);
    //        if(v3.x - centerPos.x > 0)
    //        {
    //            saPlayer.skeleton.ScaleX = -1;
    //        }
    //        else
    //        {
    //            saPlayer.skeleton.ScaleY = 1;
    //        }
    //        Move(dir3);
    //    }
    //    if(Vector2.Distance(centerPos,v3) < 0.1f && target1 && target2&&!target3)
    //     {
    //        target3 = true;
    //        InitMode = MOVEMODE.AUTO;
    //        rb.velocity = new Vector2(0, 0);
    //     }
        
    //}

    int curTarget = 0;
    void MoveToTarget()
    {
        if (InitMode != MOVEMODE.PATH) return;
        if (Vector2.Distance(centerPos, targetPointList[curTarget]) < 0.1f)
        {
            if (curTarget + 1 < targetPointList.Count) curTarget++;
            else
            {
                InitMode = MOVEMODE.AUTO;
                rb.velocity = Vector2.zero;
            }
        }
        Vector2 dir = new Vector2(targetPointList[curTarget].x - centerPos.x, targetPointList[curTarget].y - centerPos.y);
        saPlayer.skeleton.ScaleX = targetPointList[curTarget].x - centerPos.x > 0 ? -1 : 1;
        Move(dir);
    }

    IEnumerator Return2Idle()
    {
        yield return new WaitForSeconds(1f);
        PlayAnim(str_idle, true);
    }

}
