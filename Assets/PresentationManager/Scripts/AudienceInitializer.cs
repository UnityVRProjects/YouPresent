using UnityEngine;

public class AudienceInitializer : MonoBehaviour
{
    [SerializeField] GameObject AudienceMember1;
    [SerializeField] GameObject AudienceMember2;
    void Start()
    {
        for (int y = 0; y < 14; y++)
        {
            if(y != 7)
            {
                int rand = Random.Range(0, 10);
                if(rand > 5)
                {
                    Vector3 position = new Vector3(transform.position.x, 0f, transform.position.z + y * 0.488f);
                    Instantiate(AudienceMember1, position, AudienceMember1.transform.rotation, transform);
                }
                else if (rand > 1)
                {
                    Vector3 position = new Vector3(transform.position.x, 0f, transform.position.z + y * 0.488f);
                    Instantiate(AudienceMember2, position, AudienceMember2.transform.rotation, transform);
                }
            }
            
        }

        for (int x = 1; x < 8; x++)
        {
            float offsetZ = 0f;
            if(x % 2 == 1)
            {
                offsetZ = 0.28f;
            }
            for (int y = 0; y < 14; y++)
            {
                int rand = Random.Range(0, 10);
                if (rand > 5)
                {
                    Vector3 position = new Vector3(transform.position.x + x * -1.04f, 0f, transform.position.z + y * 0.488f + offsetZ);
                    Instantiate(AudienceMember1, position, AudienceMember1.transform.rotation, transform);
                }
                else if (rand > 1)
                {
                    Vector3 position = new Vector3(transform.position.x + x * -1.04f, 0f, transform.position.z + y * 0.488f + offsetZ);
                    Instantiate(AudienceMember2, position, AudienceMember2.transform.rotation, transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
