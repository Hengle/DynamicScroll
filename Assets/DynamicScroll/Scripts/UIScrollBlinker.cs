﻿using UnityEngine;
using System.Collections;

public class UIScrollBlinker : MonoBehaviour
{
	public UIScrollView scrollView;

	private bool firstTime = true;

	void Start()
	{
		if (scrollView == null)
			scrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
		
		if (scrollView != null)
		{
			if (scrollView.horizontalScrollBar != null)
				EventDelegate.Add(scrollView.horizontalScrollBar.onChange, Blink);
			
			if (scrollView.verticalScrollBar != null)
				EventDelegate.Add(scrollView.verticalScrollBar.onChange, Blink);
		}

		Blink();

		firstTime = false;
	}

	void Update()
	{
		SpringPanel sp = scrollView.gameObject.GetComponent<SpringPanel>();
		if ((sp != null && sp.enabled == true) || scrollView.currentMomentum.magnitude > 0f)
			Blink();
	}

	void OnEnable()
	{
		if (firstTime == false)
			Blink();
	}

	public void Blink()
	{
		if (scrollView.panel == null)
			return;

		Vector3[] corners = scrollView.panel.worldCorners;
		
		for (int i=0; i<4; i++)
		{
			Vector3 v = corners[i];
			v = transform.InverseTransformPoint(v);
			corners[i] = v;
		}
		
		Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);

		for (int i=0; i<transform.childCount; i++)
		{
			Transform t = transform.GetChild(i);
			if (UICamera.IsPressed(t.gameObject) == true)
				continue;

			Bounds b;

			UIWidget widget = t.GetComponent<UIWidget>();
			if (widget == null)
				b = NGUIMath.CalculateRelativeWidgetBounds(t);
			else
				b = new Bounds(Vector3.zero, new Vector3(widget.width, widget.height));

			float minX = corners[0].x - b.extents.x;
			float minY = corners[0].y - b.extents.y;
			float maxX = corners[2].x + b.extents.x;
			float maxY = corners[2].y + b.extents.y;

			float distanceX = t.localPosition.x - center.x + scrollView.panel.clipOffset.x - transform.localPosition.x;
			float distanceY = t.localPosition.y - center.y + scrollView.panel.clipOffset.y - transform.localPosition.y;

			bool active = (distanceX > minX && distanceX < maxX) && (distanceY > minY && distanceY < maxY);
			NGUITools.SetActive(t.gameObject, active, false);
		}
	}
}
