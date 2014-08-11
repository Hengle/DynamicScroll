﻿using UnityEngine;
using System.Collections;

public class UIScrollContent
{
	public string id;
	public GameObject prefab;
	
	public delegate void OnInitContent(string id, GameObject go);
	public OnInitContent onInitContent;

	public delegate void OnVisibleContent(string id, GameObject go);
	public OnVisibleContent onVisibleContent;
}
