using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UniRx;

public class Enemy_Tentacle : MonoBehaviour,IEnemy
{
    private GameObject Trigger;
    private GameObject Head;

    public SkeletonAnimation saPlayer;
    public enum EnemyDir { NONE,LEFT,RIGHT}
    [SerializeField] EnemyDir dir = EnemyDir.NONE;
    [SpineAnimation]
    public string str_idle_dong,str_born,str_idle, str_Attack, str_Die;

    private RaycastHit2D hitTarget;
    private Vector2 startPos, endPosR,endPosL,endPos,endPosBornR,endPosBornL,endPosBorn;
    private bool born;
    public float bornSpeed;
    public LayerMask lmPlayer;

    public ParticleSystem pBlood;
    public ParticleSystem pSkull;

    private void Start()
    {
        Trigger = transform.Find("Trigger").gameObject;
        Head = transform.Find("Head").gameObject;
        startPos = transform.Find("start").transform.position;

        if (dir == EnemyDir.LEFT || dir == EnemyDir.NONE)
        {
            saPlayer.skeleton.ScaleX = 1;
            endPosL = transform.Find("endL").transform.position;
            endPos = endPosL;
            endPosBornL = transform.Find("endBornL").transform.position;
            endPosBorn = endPosBornL;
        }
        else if (dir == EnemyDir.RIGHT)
        {
            saPlayer.skeleton.ScaleX = -1;
            endPosR = transform.Find("endR").transform.position;
            endPos = endPosR;
            endPosBornR = transform.Find("endBornR").transform.position;
            endPosBorn = endPosBornR;
        }

        if (MapLevelManager.Instance != null)
        {
            MapLevelManager.Instance.lstAllEnemies.Add(this);
        }

        saPlayer.AnimationState.Start += Event;
    }

    private bool player, enemy, hostage;
    private void Event(TrackEntry trackEntry)
    {
        if (saPlayer.AnimationName.Equals(str_born))
        {
            born = true;
        }
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
                    if (hitTarget.collider.gameObject.tag == "Hostage")
                    {
                        hitTarget.collider.gameObject.GetComponent<HostageManager>().OnDie_(true);
                        hitTarget.collider.gameObject.GetComponent<HostageManager>().PlayDie();
                        GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                    }
                }
                if (enemy)
                {
                    if (hitTarget.collider.gameObject.layer == 15)
                    {

                        hitTarget.collider.gameObject.GetComponentInParent<EnemyMelee>()?.OnDie_();
                        hitTarget.collider.gameObject.GetComponentInParent<EnemyRange>()?.OnDie_();
                        hitTarget.collider.gameObject.GetComponentInParent<Enemy_Scorpion>()?.OnDie();
                        hitTarget.collider.gameObject.GetComponentInParent<Enemy_Bat>()?.OnDie();
                    }
                }
            });
        }

    }

    private void Update()
    {
        if (!born)
        {
            Trigger.SetActive(false);
            Head.SetActive(false);
        }

        RaycastHit2D hitBorn = Physics2D.Linecast(startPos, endPosBorn,lmPlayer);
        if (hitBorn.collider)
        {
            if(hitBorn.collider.gameObject.tag == "BodyPlayer" || hitBorn.collider.gameObject.tag == "Hostage" || hitBorn.collider.gameObject.layer == 15)
            {
                if (!born)
                {
                    PlayAnim(str_born, false);
                    //born = true;
                    
                }
            }
        }

        if (born && !die)
        {
            Trigger.SetActive(true);
            Head.SetActive(true);
            Head.transform.position += new Vector3(0, bornSpeed, 0) * Time.deltaTime;
            if (Head.transform.localPosition.y >= 1.15) bornSpeed = 0;
        }

        if(born && hitTarget.collider == null)
        {
            PlayAnim(str_idle, true);
        }

        hitTarget = Physics2D.Linecast(startPos, endPos,lmPlayer);
        if (hitTarget.collider && born)
        {
            if(hitTarget.collider.gameObject.tag == "BodyPlayer")
            {
                player = true;
                PlayAnim(str_Attack, false);
                Head.SetActive(false);
            }
            if(hitTarget.collider.gameObject.tag == "Hostage")
            {
                hostage = true;
                PlayAnim(str_Attack, false);
                Head.SetActive(false);
            }
            if(hitTarget.collider.gameObject.layer == 15)
            {
                enemy = true;
                PlayAnim(str_Attack, false);
                Head.SetActive(false);
            }
        }
    }

   

    private void PlayAnim(string anim, bool isLoop)
    {
        if (die) return;
        if (!saPlayer.AnimationName.Equals(anim))
        {
            saPlayer.AnimationState.SetAnimation(0, anim, isLoop);
        }
    }

    private bool die;
    private void OnDie()
    {
        born = false;
        Trigger.SetActive(false);
        Head.SetActive(false);
        PlayAnim(str_Die, false);
        die = true;

        pBlood.Play();
        pSkull.Play();

        AudioManager.Instance.Play(AudioConstant.tentacle_die);

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
        //    player = true;
        //    PlayAnim(str_Attack, false);
        //}
        //if(collision.tag == "Hostage")
        //{
        //    collision.gameObject.GetComponent<HostageManager>().OnDie_(true);
        //    collision.gameObject.GetComponent<HostageManager>().PlayDie();
        //    GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
        //}
        //if(collision.gameObject.layer == 15)
        //{
        //    collision.GetComponentInParent<EnemyMelee>()?.OnDie_();
        //    collision.GetComponentInParent<EnemyRange>()?.OnDie_();
        //    //collision.GetComponentInParent<EnemyCharged>()?.OnDie_();
        //}

        if (collision.gameObject.tag == Utils.TAG_LAVA
            || collision.gameObject.tag == "Trap_Other" || collision.gameObject.tag == "Trap_Gas" || collision.gameObject.tag == "arrow")
        {
            OnDie();
            if(collision.gameObject.tag == "arrow")
            {
                AudioManager.Instance.Play(AudioConstant.dmg_shoot);
                Destroy(collision.transform.parent.gameObject);
            }
        }
        if (collision.gameObject.tag == "fire")
        {
            OnDie();
        }

        if (collision.tag == "Charged")
        {
            OnDie();
            AudioManager.Instance.Play(AudioConstant.dmg_electric);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Charged")
        {
            if (die) return;
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
}
