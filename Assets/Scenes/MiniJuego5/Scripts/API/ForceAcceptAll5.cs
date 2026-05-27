using UnityEngine.Networking;

public class ForceAcceptAll5 : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}