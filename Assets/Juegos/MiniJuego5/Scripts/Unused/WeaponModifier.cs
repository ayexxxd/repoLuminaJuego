using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace TopDown.Shooting
{
    public class WeaponModifier : MonoBehaviour
    {
        [SerializeField] private GunController gunController;

        public void TryApplyUpgrade(string word)
        {
            StartCoroutine(GetPower(word));
        }

        IEnumerator GetPower(string word)
        {
            string JOSNURL = "https://10.14.255.45:5010/getPower/" + word;

            UnityWebRequest web = UnityWebRequest.Get(JOSNURL);
            web.downloadHandler = new DownloadHandlerBuffer();
            web.certificateHandler = new ForceAcceptAll();
            yield return web.SendWebRequest();

            if (web.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error API: " + web.error);
            }
            else
            {
                PowerData power = JsonConvert.DeserializeObject<PowerData>(web.downloadHandler.text);

                if (power != null)
                {
                    int dmg = Mathf.RoundToInt(power.damage);
                    float spd = power.speed;
                    float cd = power.cooldown;

                    if (dmg > 0 || spd > 0 || cd > 0)
                        gunController.SetBulletStats(dmg, spd, cd);
                }
            }
        }
    }

    [System.Serializable]
    public class PowerData
    {
        public float damage;
        public float speed;
        public float cooldown;
    }

    [System.Serializable]
    public class BulletModifier
    {
        public string keyword;
        public int damage = 5;
        public float speed = 10f;
        public float cooldown = 0.4f;
    }
}