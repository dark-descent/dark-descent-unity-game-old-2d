using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BasePlatformBuilder : MonoBehaviour
{
	[SerializeField] Sprite leftSprite;
	[SerializeField] Sprite midSprite;
	[SerializeField] Sprite rightSprite;
	[SerializeField] Sprite asdkasjdkasjdkasd;

	[SerializeField] int width = 1;
	int prevWidth = 0;

	void Update()
	{
		if (width != prevWidth)
		{
			for (int i = 0; i < transform.childCount; i++)
				DestroyImmediate(transform.GetChild(i).gameObject);

			if (width % 2 == 0)
			{
				// int side = (width - 1) / 2;
				// GameObject c = new GameObject();
				// c.transform.SetParent(transform);
				// c.transform.localPosition = Vector3.zero;
				// c.AddComponent<SpriteRenderer>().sprite = midSprite;
			}
			else
			{
				int side = (width - 1) / 2;

				GameObject c = new GameObject();
				c.transform.SetParent(transform);
				c.transform.localPosition = Vector3.zero;
				c.AddComponent<SpriteRenderer>().sprite = midSprite;

				for (int i = 0; i < side; i++)
				{
					GameObject cc = new GameObject();
					cc.transform.SetParent(transform);
					cc.transform.localPosition = new Vector2((i + 1) * midSprite.bounds.size.x, 0f);
					cc.AddComponent<SpriteRenderer>().sprite = midSprite;
				}
				
				for (int i = 0; i < side; i++)
				{
					GameObject cc = new GameObject();
					cc.transform.SetParent(transform);
					cc.transform.localPosition = new Vector2((i + 1) * -midSprite.bounds.size.x, 0f);
					cc.AddComponent<SpriteRenderer>().sprite = midSprite;
				}
			}
			prevWidth = width;
		}
	}
}
