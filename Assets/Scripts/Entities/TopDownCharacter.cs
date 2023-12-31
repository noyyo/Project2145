using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacter : MonoBehaviour
{
    public int CurrentHP = 100;
    public int MaxHP = 100;
    public float Speed = 5;
    [SerializeField]
    private List<Arm> _arms = new List<Arm>();
    private int _currentArmIndex;
    public bool isPowerUp;
    public bool m_die = false;//유닛 사망여부확인
    public GameObject[] item;
    private HpBar _hpBar;
    private void Start()
    {
        _arms.AddRange(GetComponentsInChildren<Arm>());
        _hpBar = GetComponentInChildren<HpBar>();
    }
    public void SetCharacterInfo(int maxHp, float speed)
    {
        MaxHP = maxHp;
        CurrentHP = MaxHP;
        Speed = speed;
    }
    public void ChangeArm()
    {
        if (_arms.Count <= 0)
        {
            Debug.Log("무장이 없습니다.");
            return;
        }
        _currentArmIndex++;
        if (_currentArmIndex >= _arms.Count)
            _currentArmIndex = 0;
    }
    public Arm GetCurrentArm()
    {
        if (_arms.Count <= 0)
        {
            Debug.Log("무장이 없습니다.");
            return null;
        }
        return _arms[_currentArmIndex];
    }
    public void PowerUp()
    {
        if (isPowerUp)
            return;
        isPowerUp = true;
        foreach (Arm arm in _arms)
            arm.ProjectileType++;
    }
    public void PowerUpEnd()
    {
        if (!isPowerUp)
            return;
        isPowerUp = false;
        foreach (Arm arm in _arms)
            arm.ProjectileType--;
    }

    public void TakeDamage(int damage)//플레이어, enemy 구분하려면 virtual
    {
        if (m_die)
        {
            return;
        }

        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
            SetHpBar();
        if (CurrentHP == 0)
        {

            if (tag == "Enemy" || tag == "Boss")
            {
                GameManager.Instance.playerKill++;
                GameManager.Instance.GetExp(10);
                int itemIndex = Random.Range(0, 3);
                Instantiate(item[itemIndex]).transform.position = transform.position;
                //+유닛 죽는 애니메이션 ,사운드
                Destroy(gameObject, 0.3f);
                m_die = true;
                if (tag == "Boss")
                {
                    m_die = true;

                    GameManager.Instance.PrintResult();
                    GameManager.Instance.EndGame();
                }
            }
            else if (tag == "Player")
            {
                int life = GameManager.Instance.playerLife;
                if (GameManager.Instance.playerLife > 0)
                {
                    GameManager.Instance.playerLife--;
                    CurrentHP = MaxHP;
                    SetHpBar();
                }
                else
                {
                    m_die = true;
                    GameManager.Instance.PrintResult();
                }
            }
        }
        else
        {
            //데미지 입는 애니메이션, 사운드
        }

    }
    public void SetHpBar()
    {
        _hpBar.SetHp((float)CurrentHP / MaxHP);
    }
}
