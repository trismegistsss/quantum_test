using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

public class GrabHealhBar : MonoBehaviour
{
    [Header("Live Bar")]
    [SerializeField] SpriteRenderer _scrollBar;
    [SerializeField] GameObject _shildsBar;
    [SerializeField] GameObject[] _shilds;
    [SerializeField] TMP_Text _count;

    private float _maxXPos = 0;
    private float _minXPos = -2.368f;
    private float _startHealth = 0;

    private void Awake()
    {
       /* if(null!= _shildsBar)
            _shildsBar.SetActive(false);

            _scrollBar.transform.localPosition = Vector3.zero;
            _scrollBar.gameObject.SetActive(false);*/
        
    }

    private void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void Initialize(float ypos, float health, Color color)
    {
        _startHealth = health;
        if(null!=_count)
            _count.text = 100 + "";
        transform.localPosition = new Vector3(transform.localPosition.x, ypos, 0);


        if(null!= _shilds)
            foreach (var sh in _shilds)
                sh.SetActive(false);

            _scrollBar.gameObject.SetActive(true);
        _scrollBar.color = color;
        _scrollBar.transform.localPosition = new Vector3(_maxXPos, 0, 0);
    }

    public void UpdateCounter(float hit)
    {
        if (hit < 0) hit = 0;
        var cp = hit / _startHealth * Mathf.Abs(_minXPos);
        var sp = (Mathf.Abs(_minXPos) - cp);

        _scrollBar.transform.DOLocalMoveX(-sp, 0.25f).SetEase(Ease.Linear).Play();

        if (null != _count)
            _count.text = (int)(cp / Mathf.Abs(_minXPos)*100) + "";

    }

    public void Death()
    {
        Observable.Interval(System.TimeSpan.FromSeconds(0.5f)).Subscribe(_ =>
        {
            if (null != gameObject)
                gameObject.SetActive(false);
        }).AddTo(this);
    }
}
