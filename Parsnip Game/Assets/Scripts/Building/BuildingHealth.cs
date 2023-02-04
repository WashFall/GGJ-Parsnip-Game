using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    public float health = 100f;
    private Rigidbody m_myBody;
    private float m_explosionForce = 1000f;

    public void DamageHealth(float damage)
    {
        if (m_myBody == null)
        {
            m_myBody = GetComponent<Rigidbody>();
        }
        
        health -= damage;
        
        if (health < 1)
        {
            Destroy(GetComponent<FixedJoint>());
        }
        
        m_myBody.AddForce(( transform.position - PlayerPosition.Instance.GetPlayerTransform().position) * (m_explosionForce * m_myBody.mass));
    }
}
