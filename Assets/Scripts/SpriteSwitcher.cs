using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yarn.Unity.TLK{
public class SpriteSwitcher : MonoBehaviour {

	public static Dictionary<string, Sprite> spriteCache;

	//  and  sprites & tags
	public GameObject container;
	public Text nametag;

		[YarnCommand("setsprite")]
		public void SetSprite(string name, string expression)
		{
			string summon = name;
			if(name == "None")
			{
				container.GetComponent<Image>().sprite = GameObject.Find("DialogueRunner").GetComponent<DialogueUI>().transparent;
				return;
			}
			if(expression.ToLower() != "neutral")
			{
				summon += "_" + expression;
			}
			Sprite image = null;
			if(spriteCache == null)
				spriteCache = new Dictionary<string, Sprite>();
			if(!spriteCache.TryGetValue(summon, out image))
			{
				image = (Sprite)Resources.Load(summon, typeof(Sprite));
				spriteCache.Add(summon, image);
			}
			container.GetComponent<Image>().sprite = image;
			nametag.text = name;
		}

		[YarnCommand("setname")]
		public void SetName(string name)
		{
			nametag.text = name;
		}

	}
}

/*
using UnityEngine;
using System.Collections;

namespace Yarn.Unity.Example {

[RequireComponent (typeof (SpriteRenderer))]
public class SpriteSwitcher : MonoBehaviour {

[System.Serializable]
public struct SpriteInfo {
public string name;
public Sprite sprite;
}

public SpriteInfo[] sprites;

[YarnCommand("setsprite")]
public void UseSprite(string spriteName) {

Sprite s = null;
foreach(var info in sprites) {
if (info.name == spriteName) {
s = info.sprite;
break;
}
}
if (s == null) {
Debug.LogErrorFormat("Can't find sprite named {0}!", spriteName);
return;
}

GetComponent<SpriteRenderer>().sprite = s;
}
}

}
*/
