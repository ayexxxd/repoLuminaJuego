using UnityEngine.Networking;
namespace Login{
public class ForceAcceptAll : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}}