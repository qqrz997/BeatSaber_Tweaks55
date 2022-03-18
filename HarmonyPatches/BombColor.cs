﻿using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BombColor {
		static Color defaultColor = Color.black;

		static readonly int _SimpleColor = Shader.PropertyToID("_SimpleColor");

		[HarmonyPriority(int.MaxValue)]
		static void Prefix(MonoBehaviour __instance) {
			if(__instance.name[0] == 'C')
				return;

			__instance.name = "C";

			if(Config.Instance.bombColor == defaultColor)
				return;

			var c = __instance.transform.GetChild(0);

			if(c == null)
				return;

			// If CustomNotes "HMD Only" is active, there will be another nested child (The HMD bomb), which we should apply the color to instead
			if(c.childCount != 0)
				c = c.GetChild(0);

			c.GetComponent<Renderer>()?.material?.SetColor(_SimpleColor, Config.Instance.bombColor);
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BombNoteController), nameof(BombNoteController.Init));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
