using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using ScriptableObjects.Classes.Resources;
using UnityEngine;
using static Common.Enums;
using static Common.Enums.ResourceType;

namespace UI.ResourcesView
{
	public class ResourceView : MonoBehaviour
	{
		[SerializeField] private Transform   _resourceViewElementParent;
		[SerializeField] private ResourceViewElement _resourceViewElementPrefab;
		[SerializeField] private bool  _autoHide;
		[SerializeField] protected List<ResourceData> _showResourceData;

		private readonly Dictionary<ResourceData, ResourceViewElement> _resourceViewElements = new();

		private Sequence _flySequence;

		private void Awake() => ClearResourceViewElements();
		private void Start() => ResourceHandler.Initialize();

		private void OnEnable()
		{
			ResourceHandler.OnValueAdded += AddResourceCount;
			ResourceHandler.OnValueSet += SetResourceCount;
			ResourceHandler.OnValueSubtracted += SubtractResource;
		}

		private void OnDisable()
		{
			ResourceHandler.OnValueAdded -= AddResourceCount;
			ResourceHandler.OnValueSet -= SetResourceCount;
			ResourceHandler.OnValueSubtracted -= SubtractResource;
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.K)) { ResourceHandler.AddResource(Money, 100); }

			if (Input.GetKeyDown(KeyCode.J)) { ResourceHandler.AddResource(Resource2, 100); }

			if (Input.GetKeyDown(KeyCode.L)) { ResourceHandler.AddResource(Resource3, 100); }

			if (Input.GetKeyDown(KeyCode.M)) { ResourceHandler.TrySubtractResource(Money, 100); }
		}
#endif

		private void ClearResourceViewElements(bool immediate = false)
		{
			_resourceViewElementParent.DestroyChildren(immediate);
			_resourceViewElements.Clear();
		}

		private void AddResourceCount(ResourceType type, int value)
		{
			if (TryGetElement(type, out var element)) { AddResourceCount(element, value); }
		}

		private void SubtractResource(ResourceType type, int value)
		{
			if (TryGetElement(type, out var element)) { element.SubtractResource(value); }
		}

		private void SetResourceCount(ResourceType type, int value)
		{
			if (TryGetElement(type, out var element)) { element.SetResourceCount(value); }
		}

		private bool TryGetElement(ResourceType type, out ResourceViewElement element)
		{
			var resourceData = GetResourceData(type);

			if (resourceData != default)
			{
				if (_resourceViewElements.ContainsKey(resourceData)) { element = _resourceViewElements[resourceData]; }
				else
				{
					element = Instantiate(_resourceViewElementPrefab, _resourceViewElementParent);
					element.Init(resourceData, _autoHide);
					element.name = $"{type.ToString()} View Element";
					_resourceViewElements.Add(resourceData, element);
				}

				return true;
			}

			element = null;
			return false;
		}

		private ResourceData GetResourceData(ResourceType type)
		{
			return _showResourceData.Find(x => x.Type == type);
		}

		private void AddResourceCount(ResourceViewElement viewElement, int value)
		{
			StartCoroutine(AddResourceCountCor(viewElement, value));
		}

		private IEnumerator AddResourceCountCor(ResourceViewElement viewElement, int value)
		{
			viewElement.SetCanvasShow(true);
			yield return null;

			viewElement.AddResource(value);
		}
	}
}