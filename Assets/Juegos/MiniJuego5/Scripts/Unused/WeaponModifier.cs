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
        [SerializeField] private string apiUrl = "https://192.168.100.71:5010/getPower";

        public void TryApplyUpgrade(string word)
        {
            StartCoroutine(TryApplyUpgradeCoroutine(word));
        }

        IEnumerator TryApplyUpgradeCoroutine(string word)
        {
            if (gunController == null)
            {
                gunController = FindAnyObjectByType<GunController>();
            }
            if (gunController == null)
            {
                yield break;
            }

            // --- Try API directly ---
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

                    if (dmg > 0 || spd > 0 || cd > 0)
                    {
                        gunController.SetBulletStats(dmg, spd, cd);
                        apiSuccess = true;
                    }
                }
            }

            if (apiSuccess)
                yield break;

            // --- Fallback to local modifiers ---
            string lower = word.ToLowerInvariant();
            foreach (var mod in modifiers)
            {
                if (mod.keyword.ToLowerInvariant() == lower)
                {
                    gunController.SetBulletStats(mod.damage, mod.speed, mod.cooldown);
                    yield break;
                }
            }
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

            yield return web.SendWebRequest();

            if (web.result != UnityWebRequest.Result.Success)
            {
                callback?.Invoke(null);
            }
            else
            {
                string json = web.downloadHandler.text;
                
                PowerData power = null;
                try
                {
                    power = JsonConvert.DeserializeObject<PowerData>(json);
                }
                catch (System.Exception)
                {
                }

                if (power == null)
                {
                    try
                    {
                        power = UnityEngine.JsonUtility.FromJson<PowerData>(json);
                    }
                    catch (System.Exception)
                    {
                    }
                }

                callback?.Invoke(power);
            }
            web.Dispose();
        }
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
