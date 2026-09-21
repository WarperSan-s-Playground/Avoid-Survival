using UnityEngine;
using UnityEngine.UI;

public class OtherHealthBarManager : MonoBehaviour
{
    public Slider healthBar;
    public Gradient healthColorGradient;
    public Vector3 healthBarOffset;
    public int healthBarCountDown;

    private float countdown;
    [SerializeField] private bool protectTarget = false;

    private void Start()
    {
        healthBar = transform.GetChild(0).GetComponent<Slider>();

        if (!protectTarget)
            healthBar.transform.localPosition = Camera.main.WorldToScreenPoint(transform.parent.position + healthBarOffset);
    }

    public void UpdateHealthBar(int health)
    {
        healthBar.gameObject.SetActive(health < healthBar.maxValue);
        healthBar.value = health;

        healthBar.fillRect.GetComponentInChildren<Image>().color = healthColorGradient.Evaluate(health);

        countdown = 3f;
    }

    private void Update()
    {
        if (protectTarget)
            return;

        healthBar.transform.position = Vector2.MoveTowards(healthBar.transform.position, Camera.main.WorldToScreenPoint(transform.parent.position + healthBarOffset), 100);

        countdown -= Time.deltaTime;

        if (countdown <= 0f)
        {
            healthBar.gameObject.SetActive(false);
        }
    }
}
