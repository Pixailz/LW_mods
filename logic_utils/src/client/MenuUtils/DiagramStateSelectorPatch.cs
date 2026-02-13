using System;
using System.Collections.Generic;
using System.Linq;
using LogicWorld.Interfaces;
using LogicWorld.SharedCode.Components;
using UnityEngine;

namespace PixLogicUtils.Client.Menus
{
	internal static class DiagramStateSelectorPatch
	{
		public static bool SetInputsReferencePrefix(object __instance, IComponentClientCode inputsReference)
		{
			if (inputsReference is ComponentClientCodeWrapper)
			{
				SetInputsReferenceForWrapper(__instance, inputsReference);
				return false;
			}

			return true;
		}

		private static void SetInputsReferenceForWrapper(object instance, IComponentClientCode inputsReference)
		{
			var type = instance.GetType();

			// Get the InputGraphicPool field
			var poolField = type.GetField("InputGraphicPool", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var pool = poolField?.GetValue(instance);

			// Get InputGraphicsParent
			var parentField = type.GetField("InputGraphicsParent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var parent = parentField?.GetValue(instance) as RectTransform;

			// Get ActiveInputs list
			var activeInputsField = type.GetField("ActiveInputs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var activeInputs = activeInputsField?.GetValue(instance);

			if (pool == null || parent == null || activeInputs == null)
				return;

			// Create uniform inputs (all same size)
			var getMethod = pool.GetType().GetMethod("Get", new[] { typeof(Transform) });
			var addMethod = activeInputs.GetType().GetMethod("Add");

			for (int i = 0; i < inputsReference.InputCount; i++)
			{
				var diagramInput = getMethod?.Invoke(pool, new object[] { parent });
				if (diagramInput != null)
				{
					addMethod?.Invoke(activeInputs, new[] { diagramInput });
				}
			}
		}
	}
}
