using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace TopDown.Shooting
{
    public class WeaponModifier : MonoBehaviour
    {
        [SerializeField] private GunController gunController;
        [SerializeField] private BulletModifier[] modifiers;
        [SerializeField] private string apiUrl = "https://10.14.255.45:5010/getPower";

        public void TryApplyUpgrade(string word)
        {
            StartCoroutine(TryApplyUpgradeCoroutine(word));
        }

        IEnumerator TryApplyUpgradeCoroutine(string word)
        {
            bool apiSuccess = false;
            if (!string.IsNullOrEmpty(apiUrl))
            {
                PowerData power = null;
                yield return FetchFromApi(word, (result) => power = result);

                if (power != null)
                {
                    int dmg = Mathf.RoundToInt(power.damage);
                    float spd = power.speed;
                    float cd = power.cooldown;

                    gunController.SetBulletStats(dmg, spd, cd);
                    apiSuccess = true;
                }
            }

            if (apiSuccess)
                yield break;
        }

        private IEnumerator FetchFromApi(string word, System.Action<PowerData> callback)
        {
            string jsonBody = "{\"nombre\":\"" + word + "\"}";

            UnityWebRequest web = new UnityWebRequest(apiUrl, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            web.uploadHandler = new UploadHandlerRaw(bodyRaw);
            web.downloadHandler = new DownloadHandlerBuffer();
            web.SetRequestHeader("Content-Type", "application/json");
            web.certificateHandler = new ForceAcceptAll();
            Debug.Log(web.certificateHandler);
            yield return web.SendWebRequest();


    }

    [System.Serializable]
    public class BulletModifier
    {
        public string keyword;
        public int damage = 5;
        public float speed = 10f;
        public float cooldown = 0.4f;
    }
    }}