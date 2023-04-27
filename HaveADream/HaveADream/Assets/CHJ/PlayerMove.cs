using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : Singleton<PlayerMove>

{

    //스피드 변수
    [SerializeField] float speed = 1;


    [SerializeField] GameObject HpBar;
    [SerializeField] Image HpBarFilled;

    bool isTouching = false;
    bool isSkillBtn = false;

    public bool isShield = false;


    Rigidbody2D rb;
    SpriteRenderer sr;

    private SceneManager sm = null;


    [SerializeField] float smoothTime; // 보간 시간

    private Vector3 targetPosition; // 목표 위치
    private Vector3 velocity = Vector3.zero; // 보간용 변수

    public GameObject MagneticField;

    [SerializeField] GameObject shieldSprite;

    [SerializeField] GameObject ResultWindow;
    [SerializeField] GameObject Player;

    //날개 스킬 변수
    private const int maxWingCnt = 4;
    public int wingCnt = 0;

    public int resultStarCnt = 0;

    [SerializeField] int maxBulletsPerShot; // 발사할 총알 수 제한
    private int bulletsFired = 0; // 발사된 총알 수

    //[SerializeField] GameObject resultWindow;

    //총알 발사 카메라
    private Camera mainCam;



    public float GetPlayerSpeed() { return speed; }

    //스킬 사용중 무적 상태를 위한 함수
    public void ChangeLayer(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
    }

    //색상 변화 함수
    public void ChangeColor()
    {
        this.sr.color = new Color(1, 1, 1, 1);
    }


    //스킬1 : 스피드 상승
    public void ChangeSpeed(float speed = 1)
    {
        this.speed = speed;
    }

    //스킬2 : 안 보이게 되는 무적 부스터
    public void ChangeInvisible()
    {
        //this.gameObject.layer = 21;
        ChangeLayer(Player, 21);

        //Debug.Log("21로 전환되었나?");
        sr.color = new Color(0.7f, 0.4f, 0.4f, 1f);
        MapMove.Instance.mapSpeed = 12f;

    }


    //스킬3 : 체력 회복
    public void ChangeHealth()
    {
        DataManager.Instance.HealthCurrent += 1.0f;
        HpBarFilled.fillAmount += 0.1f;
    }

    //스킬4 : 자석
    public void Magnet()
    {
        //MagneticField.layer = 21;
        MagneticField.SetActive(true);
    }

    //스킬5 : 방어막 (구현 완료, 건드리지 말 것)
    public void Shield()
    {
        //this.gameObject.layer = 21;

        //실드 작동 확인
        //sr.color = new Color(0.3f, 0.4f, 0.7f, 1f);
        shieldSprite.SetActive(true);
        isShield = true;
    }

    public void setIsSkillBtn(bool flag)
    {
        isSkillBtn = flag;
    }







    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        GetStageData();
    }
    private void OnEnable()
    {
        //씬 시작 시 Hp바 초기화
        HpBarFilled.fillAmount = 1.0f;
        wingCnt = 0;
        bulletsFired = 0;
        //실드 초기화
        isShield = false;
        shieldSprite.SetActive(false);
        GetStageData();



    }
    void GetStageData()
    {
        string key = UserDataManager.Instance.GetUserData_userCurrentStage(); // 유저가 선택한 스테이지 key
        maxBulletsPerShot = (int)StageDataManager.Instance.GetStageInfo(key)["dreapiece_req_count"];
    }

    void Start()
    {

        sm = SceneManager.Instance;
        targetPosition = transform.position + (Vector3.right * 2f); // 목표 위치 설정
    }


    void Update()
    {
        // 주석이 다 이상해졌는데 
        if (Input.GetMouseButton(0) && !isSkillBtn) isTouching = true;
        else isTouching = false;
    }
    private void FixedUpdate()
    {
        if (isTouching == false)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 18f * speed);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 18f * speed);
        }
    }

    private void OnDisable()
    {
        maxBulletsPerShot = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //상자 충돌
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            OnDamaged();

            //hp바
            if (HpBarFilled.fillAmount <= 0.0f)
            {
                ResultWindow.SetActive(true);
            }
            //실드
            if (isShield)
            {
                HpBarFilled.fillAmount += 0.25f;
                DataManager.Instance.HealthCurrent += 2.5f;
                isShield = false;
                shieldSprite.SetActive(false);
            }

        }
        //날개 충돌
        if (collision.gameObject.tag == "Wing")
        {
            if (wingCnt < maxWingCnt)
            {
                wingCnt++;
                // 아이템을 먹는 코드 추가
                MapMove.Instance.mapSpeed += 1.0f;
            }
        }
        //결과창
        if (collision.gameObject.tag == "Result1Star")
        {
            DataManager.Instance.ResultStars += 1;
        }
        if (collision.gameObject.tag == "Result2Star")
        {
            DataManager.Instance.ResultStars += 1;

            if (DataManager.Instance.DreamPieceScore < maxBulletsPerShot)
            {
                ResultWindow.SetActive(true);
                DistanceManager.Instance.DistanceUI_OFF();
            }
            else if (DataManager.Instance.DreamPieceScore >= maxBulletsPerShot)
            {
                Invoke("ShootBullet", 1f);
            }
        }
    }
    void OnDamaged()
    {
        //layer 바꿔서 충돌하지 않도록 임시로 바꿈
        gameObject.layer = 21;
        //체력바 감소
        HpBarFilled.fillAmount -= 0.25f;
        //데이터상 감소
        DataManager.Instance.HealthCurrent -= 2.5f;
        //맵 속도 조절
        if (!isShield)
        {
            MapMove.Instance.mapSpeed = 5f;
            wingCnt = 0;
        }
        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.PlayerDamaged);

        sr.color = new Color(1, 1, 1, 0.4f);
        Invoke("OffDamaged", 2f);
    }

    void OffDamaged()
    {
        gameObject.layer = 20;
        sr.color = new Color(1, 1, 1, 1);
    }

    void shootBossProjectile()
    {
        //var direction = Vector2.right;
        //GameObject projectile = Instantiate(bossProjectile, transform.position, transform.rotation);
        var bullet = BulletPool.GetObject();
        bullet.Shoot();
        bulletsFired++;


        // 사운드
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.PlayerAttack);

        if (bulletsFired >= maxBulletsPerShot)
        {
            CancelInvoke("shootBossProjectile");

        }

    }
    void ShootBullet()
    {
        if (bulletsFired < maxBulletsPerShot)
        {
            InvokeRepeating("shootBossProjectile", 2f, 0.3f);

        }
        //멈추기
        else
        {
            CancelInvoke("shootBossProjectile");
        }

    }
}
