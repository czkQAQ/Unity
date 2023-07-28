using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UniRx;

public class Enemy_Scorpion : MonoBehaviour, IEnemy
{
    Rigidbody2D rig;
    [SerializeField] LayerMask layer;
    [SerializeField, ReadOnly] Vector2 dir;
    [SerializeField, ReadOnly] Vector2 groundDir;
    [SerializeField] float length;
    [SerializeField] float speed;
    [SerializeField] Vector3 offset;
    [SerializeField] Transform ani;
    [SerializeField] float minValue;
    bool canChange = true;


    public float radius;
    public float radius2;
    [SerializeField] LayerMask lmPlayer;
    private bool hasTarget;
    public bool die;

    public SkeletonAnimation saPlayer;
    [SpineAnimation]
    public string str_idle, str_att, str_run, str_die;
    private bool canMove;

    public GameObject Trigger;

    public GameObject stick;

    public GameObject platform;
    private Vector3 platformTrans;
    private bool platformMove;
    [SerializeField] private float correctionOffset = 0.6f;

    private GameObject Colider;

    public ParticleSystem pBlood;
    public ParticleSystem pSkull;
    private System.IDisposable disposable;
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        dir = Vector2.right * transform.localScale.x;
        groundDir = Vector2.down;

        Trigger = transform.Find("Trigger").gameObject;

        Colider = transform.Find("Collider").gameObject;
        Colider.SetActive(false);

        canMove = true;


    }

    private void Start()
    {
        if (platform != null)
        {
            platformTrans = platform.transform.position;
        }

        saPlayer.AnimationState.Start += Event;

        if (MapLevelManager.Instance != null)
        {
            MapLevelManager.Instance.lstAllEnemies.Add(this);
        }
    }

    private bool player, enemy, hostage;
    private void Event(TrackEntry trackEntry)
    {
        if (saPlayer.AnimationName.Equals(str_att))
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.acMeleeAttack);
            }//怪物攻击音效

            var time = saPlayer.skeleton.Data.FindAnimation(str_att).Duration;
            disposable = Observable.Timer(System.TimeSpan.FromSeconds(time * 0.55f)).Subscribe(_ =>
            {
                if (player)
                {

                    PlayerManager.Instance.OnPlayerDie(true);
                    GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;

                }
                if (hostage)
                {
                    if (hitTarget.gameObject.tag == "Hostage")
                    {
                        hitTarget.gameObject.GetComponent<HostageManager>().OnDie_(true);
                        hitTarget.gameObject.GetComponent<HostageManager>().PlayDie();
                        GameManager.Instance.gameState = GameManager.GAMESTATE.LOSE;
                    }
                }
                if (enemy)
                {
                    if (hitTarget.gameObject.layer == 15)
                    {
                        hitTarget.gameObject.GetComponentInParent<EnemyMelee>()?.OnDie_();
                        hitTarget.gameObject.GetComponentInParent<EnemyRange>()?.OnDie_();
                    }
                }
            });
        }
    }


    Collider2D hitTarget;
    RaycastHit2D hit;
    private bool stickMove;
    private void Update()
    {
        if (GameManager.Instance.gPanelWin.gameObject.activeSelf)
        {
            disposable?.Dispose();
        }
        if (groundDir.normalized == Vector2.down || die)
        {
            rig.gravityScale = 1;
        }
        else
        {
            rig.gravityScale = 0;
        }
        var hitPlayer = Physics2D.OverlapCircle(transform.position, radius, lmPlayer);
        if (hitPlayer != null)
        {
            hasTarget = true;
        }

        if (!hasTarget) return;
        if (die) return;
        if (stick == null || stick?.GetComponent<Rigidbody2D>().velocity != Vector2.zero) stickMove = true;

        if (platform == null || platform.transform.position != platformTrans) platformMove = true;
        if (!platformMove) return;

        if (!stickMove) return;
        Clamp();

        if (canMove)
        {
            rig.velocity = dir * speed;
            PlayAnim(str_run, true);
        }


        hitTarget = Physics2D.OverlapCircle(transform.position, radius2, lmPlayer);
        if (hitTarget.tag == "BodyPlayer")
        {
            if (!PlayerManager.Instance.isTakeSword)
            {
                player = true;
                canMove = false;
                rig.velocity = Vector2.zero;
                PlayAnim(str_att, false);
                StartCoroutine(Return2Idle());
            }
            else if (PlayerManager.Instance.isTakeSword)
            {
                canMove = false;
                PlayAnim(str_idle, true);
                PlayerManager.Instance.PlayAnim(PlayerManager.Instance.str_Att, false);
                Invoke("OnDie", 0.7f);
            }
        }
        if (hitTarget.tag == "Hostage")
        {
            hostage = true;
            canMove = false;
            rig.velocity = Vector2.zero;
            PlayAnim(str_att, false);
            StartCoroutine(Return2Idle());
        }
        if (hitTarget.gameObject.layer == 15 && hitTarget.transform.parent.name != "Enemy_Scorpion")
        {
            enemy = true;
            canMove = false;
            rig.velocity = Vector2.zero;
            PlayAnim(str_att, false);
            StartCoroutine(Return2Idle());
        }
    }

    void Clamp()
    {
        var hit = Physics2D.Raycast(transform.position, dir, length, layer);
        var ground1 = Physics2D.Raycast(transform.position + offset, groundDir, length, layer);
        var ground2 = Physics2D.Raycast(transform.position - offset, groundDir, length, layer);
        if (hit)
        {
            var groundHit = Physics2D.Raycast(transform.position, -hit.normal, minValue, layer);
            if (!groundHit) return;

            if (transform.localScale.x == 1)
            {
                dir = new Vector2(hit.normal.y, -hit.normal.x);//检测方向旋转
                offset = new Vector2(offset.y, -offset.x);//地面检测方向旋转
                ani.eulerAngles += new Vector3(0, 0, 90);//骨骼动画旋转
            }
            else
            {
                dir = new Vector2(-hit.normal.y, hit.normal.x);
                offset = new Vector2(offset.y, -offset.x);
                ani.eulerAngles -= new Vector3(0, 0, 90);
            }
            groundDir = -hit.normal;
            transform.position += (Vector3)dir * correctionOffset;
        }
        else if (!ground1 && !ground2 && canChange)//如果没有检测到地面（站在了墙顶？），向下移动
        {
            canChange = false;
            dir = groundDir;
            if (transform.localScale.x == 1)
            {
                ani.eulerAngles -= new Vector3(0, 0, 90);
                groundDir = new Vector2(groundDir.y, -groundDir.x);
                offset = new Vector2(offset.y, -offset.x);
            }
            else
            {
                ani.eulerAngles += new Vector3(0, 0, 90);
                groundDir = new Vector2(-groundDir.y, groundDir.x);
                offset = new Vector2(-offset.y, offset.x);
            }
            transform.position += (Vector3)dir * correctionOffset;
        }
        else if ((ground1 || ground2) && !canChange)
        {
            canChange = true;
        }
    }

    IEnumerator Return2Idle()
    {
        yield return new WaitForSeconds(1.333f);
        PlayAnim(str_idle, true);
    }

    private void PlayAnim(string anim, bool isLoop)
    {
        if (!saPlayer.AnimationName.Equals(anim))
        {
            saPlayer.AnimationState.SetAnimation(0, anim, isLoop);
        }
    }

    public void OnDie()
    {
        die = true;
        PlayAnim(str_die, false);
        Trigger.SetActive(false);
        this.gameObject.layer = 22;
        rig.gravityScale = 1;

        Colider.SetActive(true);
        transform.gameObject.layer = 23;

        pBlood.Play();
        pSkull.Play();

        AudioManager.Instance.Play(AudioConstant.scorpion_die);


        //transform.GetComponent<CircleCollider2D>().enabled = false;
        MapLevelManager.Instance.lstAllEnemies.Remove(this);
        if (MapLevelManager.Instance.questType == MapLevelManager.QUEST_TYPE.KILL && MapLevelManager.Instance.lstAllEnemies.Count == 0)
        {
            PlayerManager.Instance.OnWin(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Utils.TAG_LAVA
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

        if (collision.tag == "Charged")
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, radius2);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, dir * length);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + offset, groundDir * length);
        Gizmos.DrawRay(transform.position - offset, groundDir * length);
    }

    private void OnDestroy()
    {
        disposable?.Dispose();
    }
}



