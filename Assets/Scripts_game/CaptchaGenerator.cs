using UnityEngine ;

[CreateAssetMenu]
public class CaptchaGenerator : ScriptableObject {
   public Captcha[] captchas ;

   public static int index = 0 ;

    public Captcha Generate() // toma uno de los captcha al azar de los disponibles
    {
        int randomIndex = Random.Range(0, captchas.Length);
        Debug.Log($"Captcha generado: index={randomIndex}, valor={captchas[randomIndex].Value}");
        return captchas[randomIndex];
    }


    public bool IsCodeValid (string input, Captcha c) // chequea si es válido el texto ingresado 
    {
      return (input == c.Value) ;
    }
}
