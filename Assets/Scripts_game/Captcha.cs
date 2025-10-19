using UnityEngine ;
using System ;

[Serializable]
public class Captcha {
   public Sprite Image ;
   public string Value ;
}
// Esto es el serializable del captcha de texto, es como el struct básico que ocupara para clasificar cada imagen y su texto