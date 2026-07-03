using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GameControls : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	public struct GameplayActions
	{
		private GameControls m_Wrapper;

		public InputAction MoveP1 => m_Wrapper.m_Gameplay_MoveP1;

		public InputAction MoveP2 => m_Wrapper.m_Gameplay_MoveP2;

		public InputAction MoveP3 => m_Wrapper.m_Gameplay_MoveP3;

		public InputAction UpP1 => m_Wrapper.m_Gameplay_UpP1;

		public InputAction UpP2 => m_Wrapper.m_Gameplay_UpP2;

		public InputAction UpP3 => m_Wrapper.m_Gameplay_UpP3;

		public InputAction DownP1 => m_Wrapper.m_Gameplay_DownP1;

		public InputAction DownP2 => m_Wrapper.m_Gameplay_DownP2;

		public InputAction DownP3 => m_Wrapper.m_Gameplay_DownP3;

		public InputAction Pause => m_Wrapper.m_Gameplay_Pause;

		public bool enabled => Get().enabled;

		public GameplayActions(GameControls wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Gameplay;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(GameplayActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IGameplayActions instance)
		{
			if (instance != null && !m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
				MoveP1.started += instance.OnMoveP1;
				MoveP1.performed += instance.OnMoveP1;
				MoveP1.canceled += instance.OnMoveP1;
				MoveP2.started += instance.OnMoveP2;
				MoveP2.performed += instance.OnMoveP2;
				MoveP2.canceled += instance.OnMoveP2;
				MoveP3.started += instance.OnMoveP3;
				MoveP3.performed += instance.OnMoveP3;
				MoveP3.canceled += instance.OnMoveP3;
				UpP1.started += instance.OnUpP1;
				UpP1.performed += instance.OnUpP1;
				UpP1.canceled += instance.OnUpP1;
				UpP2.started += instance.OnUpP2;
				UpP2.performed += instance.OnUpP2;
				UpP2.canceled += instance.OnUpP2;
				UpP3.started += instance.OnUpP3;
				UpP3.performed += instance.OnUpP3;
				UpP3.canceled += instance.OnUpP3;
				DownP1.started += instance.OnDownP1;
				DownP1.performed += instance.OnDownP1;
				DownP1.canceled += instance.OnDownP1;
				DownP2.started += instance.OnDownP2;
				DownP2.performed += instance.OnDownP2;
				DownP2.canceled += instance.OnDownP2;
				DownP3.started += instance.OnDownP3;
				DownP3.performed += instance.OnDownP3;
				DownP3.canceled += instance.OnDownP3;
				Pause.started += instance.OnPause;
				Pause.performed += instance.OnPause;
				Pause.canceled += instance.OnPause;
			}
		}

		private void UnregisterCallbacks(IGameplayActions instance)
		{
			MoveP1.started -= instance.OnMoveP1;
			MoveP1.performed -= instance.OnMoveP1;
			MoveP1.canceled -= instance.OnMoveP1;
			MoveP2.started -= instance.OnMoveP2;
			MoveP2.performed -= instance.OnMoveP2;
			MoveP2.canceled -= instance.OnMoveP2;
			MoveP3.started -= instance.OnMoveP3;
			MoveP3.performed -= instance.OnMoveP3;
			MoveP3.canceled -= instance.OnMoveP3;
			UpP1.started -= instance.OnUpP1;
			UpP1.performed -= instance.OnUpP1;
			UpP1.canceled -= instance.OnUpP1;
			UpP2.started -= instance.OnUpP2;
			UpP2.performed -= instance.OnUpP2;
			UpP2.canceled -= instance.OnUpP2;
			UpP3.started -= instance.OnUpP3;
			UpP3.performed -= instance.OnUpP3;
			UpP3.canceled -= instance.OnUpP3;
			DownP1.started -= instance.OnDownP1;
			DownP1.performed -= instance.OnDownP1;
			DownP1.canceled -= instance.OnDownP1;
			DownP2.started -= instance.OnDownP2;
			DownP2.performed -= instance.OnDownP2;
			DownP2.canceled -= instance.OnDownP2;
			DownP3.started -= instance.OnDownP3;
			DownP3.performed -= instance.OnDownP3;
			DownP3.canceled -= instance.OnDownP3;
			Pause.started -= instance.OnPause;
			Pause.performed -= instance.OnPause;
			Pause.canceled -= instance.OnPause;
		}

		public void RemoveCallbacks(IGameplayActions instance)
		{
			if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IGameplayActions instance)
		{
			foreach (IGameplayActions gameplayActionsCallbackInterface in m_Wrapper.m_GameplayActionsCallbackInterfaces)
			{
				UnregisterCallbacks(gameplayActionsCallbackInterface);
			}
			m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct MenusActions
	{
		private GameControls m_Wrapper;

		public InputAction Horizontal => m_Wrapper.m_Menus_Horizontal;

		public InputAction Submit => m_Wrapper.m_Menus_Submit;

		public InputAction Pause => m_Wrapper.m_Menus_Pause;

		public bool enabled => Get().enabled;

		public MenusActions(GameControls wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Menus;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(MenusActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IMenusActions instance)
		{
			if (instance != null && !m_Wrapper.m_MenusActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_MenusActionsCallbackInterfaces.Add(instance);
				Horizontal.started += instance.OnHorizontal;
				Horizontal.performed += instance.OnHorizontal;
				Horizontal.canceled += instance.OnHorizontal;
				Submit.started += instance.OnSubmit;
				Submit.performed += instance.OnSubmit;
				Submit.canceled += instance.OnSubmit;
				Pause.started += instance.OnPause;
				Pause.performed += instance.OnPause;
				Pause.canceled += instance.OnPause;
			}
		}

		private void UnregisterCallbacks(IMenusActions instance)
		{
			Horizontal.started -= instance.OnHorizontal;
			Horizontal.performed -= instance.OnHorizontal;
			Horizontal.canceled -= instance.OnHorizontal;
			Submit.started -= instance.OnSubmit;
			Submit.performed -= instance.OnSubmit;
			Submit.canceled -= instance.OnSubmit;
			Pause.started -= instance.OnPause;
			Pause.performed -= instance.OnPause;
			Pause.canceled -= instance.OnPause;
		}

		public void RemoveCallbacks(IMenusActions instance)
		{
			if (m_Wrapper.m_MenusActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IMenusActions instance)
		{
			foreach (IMenusActions menusActionsCallbackInterface in m_Wrapper.m_MenusActionsCallbackInterfaces)
			{
				UnregisterCallbacks(menusActionsCallbackInterface);
			}
			m_Wrapper.m_MenusActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public interface IGameplayActions
	{
		void OnMoveP1(InputAction.CallbackContext context);

		void OnMoveP2(InputAction.CallbackContext context);

		void OnMoveP3(InputAction.CallbackContext context);

		void OnUpP1(InputAction.CallbackContext context);

		void OnUpP2(InputAction.CallbackContext context);

		void OnUpP3(InputAction.CallbackContext context);

		void OnDownP1(InputAction.CallbackContext context);

		void OnDownP2(InputAction.CallbackContext context);

		void OnDownP3(InputAction.CallbackContext context);

		void OnPause(InputAction.CallbackContext context);
	}

	public interface IMenusActions
	{
		void OnHorizontal(InputAction.CallbackContext context);

		void OnSubmit(InputAction.CallbackContext context);

		void OnPause(InputAction.CallbackContext context);
	}

	private readonly InputActionMap m_Gameplay;

	private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();

	private readonly InputAction m_Gameplay_MoveP1;

	private readonly InputAction m_Gameplay_MoveP2;

	private readonly InputAction m_Gameplay_MoveP3;

	private readonly InputAction m_Gameplay_UpP1;

	private readonly InputAction m_Gameplay_UpP2;

	private readonly InputAction m_Gameplay_UpP3;

	private readonly InputAction m_Gameplay_DownP1;

	private readonly InputAction m_Gameplay_DownP2;

	private readonly InputAction m_Gameplay_DownP3;

	private readonly InputAction m_Gameplay_Pause;

	private readonly InputActionMap m_Menus;

	private List<IMenusActions> m_MenusActionsCallbackInterfaces = new List<IMenusActions>();

	private readonly InputAction m_Menus_Horizontal;

	private readonly InputAction m_Menus_Submit;

	private readonly InputAction m_Menus_Pause;

	public InputActionAsset asset { get; }

	public InputBinding? bindingMask
	{
		get
		{
			return asset.bindingMask;
		}
		set
		{
			asset.bindingMask = value;
		}
	}

	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return asset.devices;
		}
		set
		{
			asset.devices = value;
		}
	}

	public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

	public IEnumerable<InputBinding> bindings => asset.bindings;

	public GameplayActions Gameplay => new GameplayActions(this);

	public MenusActions Menus => new MenusActions(this);

	public GameControls()
	{
		asset = InputActionAsset.FromJson("{\n    \"name\": \"PlayerControls\",\n    \"maps\": [\n        {\n            \"name\": \"Gameplay\",\n            \"id\": \"1abf14f0-66c3-4b89-ae7c-d9c3c00360d6\",\n            \"actions\": [\n                {\n                    \"name\": \"MoveP1\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"28f6d0ea-5eda-460d-ab4d-04bf5b8c63f2\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"MoveP2\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"c5bb9a12-9df7-4446-b713-0ba5f518f751\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"MoveP3\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"4831a9e7-72fd-45a8-9405-3550bd27907b\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"UpP1\",\n                    \"type\": \"Button\",\n                    \"id\": \"f26009a2-33bc-4554-a0f1-fa474880a4f3\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"UpP2\",\n                    \"type\": \"Button\",\n                    \"id\": \"cade0096-bbd6-4083-9feb-a1f72ee3eb3f\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"UpP3\",\n                    \"type\": \"Button\",\n                    \"id\": \"1d73fa3a-5469-49ab-924c-3a3677d8a0fe\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"DownP1\",\n                    \"type\": \"Button\",\n                    \"id\": \"a5f8818d-92c2-4e1f-934a-774d118775dd\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"DownP2\",\n                    \"type\": \"Button\",\n                    \"id\": \"61d91dc5-a9bd-4507-8764-81b6191de6e2\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"DownP3\",\n                    \"type\": \"Button\",\n                    \"id\": \"6c7d3ccc-f287-4f6f-883f-102a83752542\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Pause\",\n                    \"type\": \"Button\",\n                    \"id\": \"9c504e17-12c5-4d92-b21c-e16d45f40cd7\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"44088e54-445a-49f3-a910-e054bdfe5f6e\",\n                    \"path\": \"<Gamepad>/leftStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"p1Arrows\",\n                    \"id\": \"2656e305-2022-4c20-b0fe-5b88bf6517a9\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP1\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Up\",\n                    \"id\": \"7f12d3ef-10c8-4ec3-ae3a-183efe7e2763\",\n                    \"path\": \"<Keyboard>/upArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Down\",\n                    \"id\": \"e583bbd2-5381-4895-bc53-5f3a04d0366a\",\n                    \"path\": \"<Keyboard>/downArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Left\",\n                    \"id\": \"75a7e1f9-c552-4460-8b9b-f986bd9559a9\",\n                    \"path\": \"<Keyboard>/leftArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Right\",\n                    \"id\": \"a9656450-76d3-4755-ac73-15044e155eec\",\n                    \"path\": \"<Keyboard>/rightArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"03737583-a85b-4259-bc9d-5ac0de223587\",\n                    \"path\": \"<Gamepad>/leftStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"2D Vector\",\n                    \"id\": \"485c4e51-80af-4574-a659-0481aaf0b4df\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP2\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Up\",\n                    \"id\": \"02bebbbc-083d-41ee-a03d-19a03c0b569b\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Down\",\n                    \"id\": \"7e578e21-8c1e-4a6b-8975-a2c5b488d48a\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Left\",\n                    \"id\": \"3cdef08b-6321-481d-85b4-ef2c8603d988\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Right\",\n                    \"id\": \"58a2ff47-914e-49ee-b28a-6e472d66d507\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"2D Vector\",\n                    \"id\": \"f104f282-f94b-495a-91b0-06a50deda75c\",\n                    \"path\": \"2DVector\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP3\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Up\",\n                    \"id\": \"616304ef-24d9-4010-b52b-929f19f064b8\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Down\",\n                    \"id\": \"c0636c54-5d42-4db4-8095-b071dfac8541\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Left\",\n                    \"id\": \"d26945ba-7f42-444f-8a26-ccd195eaaad7\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Right\",\n                    \"id\": \"fd3f03e9-6f0c-4eda-a874-16d9e4b16221\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"MoveP3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"fee46d6a-c99d-4264-8fd6-81c6f8313770\",\n                    \"path\": \"<Keyboard>/upArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"UpP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"7dfc20f2-e75f-42ee-8a86-03600140a97e\",\n                    \"path\": \"<Gamepad>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"UpP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1cef6ebe-6d3e-410e-960c-affb26ad47a9\",\n                    \"path\": \"<Gamepad>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"UpP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"b29f0519-13db-4684-8b9e-e50c788d28f5\",\n                    \"path\": \"<Gamepad>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"UpP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3ac0d448-8eda-4130-bf79-cbfd4a58fa05\",\n                    \"path\": \"<Gamepad>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"UpP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"a4e03882-4210-4f00-8726-b925a0addfbf\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"UpP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"ae4aa109-cc8b-4a74-8885-5bd391587ed1\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"UpP3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"9e949982-88ae-42ee-b68e-9d4ca73f70a4\",\n                    \"path\": \"<Gamepad>/rightShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"DownP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c8d24488-0ac1-4bc3-8e5d-8ab8a0ddb5ec\",\n                    \"path\": \"<Keyboard>/downArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"DownP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"b8cffe75-c87d-4e49-baea-b875a85e6c57\",\n                    \"path\": \"<Gamepad>/buttonEast\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"DownP1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"e09ae3a5-f9e2-4345-846b-c024f5088c55\",\n                    \"path\": \"<Gamepad>/rightShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"DownP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"da14432e-a5ee-49fa-9422-380781e2200a\",\n                    \"path\": \"<Gamepad>/buttonEast\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"DownP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"04fc1e45-502f-411d-b47e-cd149b56ae90\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"DownP2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"62bb8dd4-1d1f-4707-b036-e3cf36ea7148\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"DownP3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"a48d33ef-8c60-4b79-811d-b044449289cc\",\n                    \"path\": \"<Gamepad>/start\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Pause\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Menus\",\n            \"id\": \"649fb2c7-74dd-4447-bcab-d16b647cb6e7\",\n            \"actions\": [\n                {\n                    \"name\": \"Horizontal\",\n                    \"type\": \"Value\",\n                    \"id\": \"4f13f877-23e0-4f56-9883-4a6ff3487127\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Submit\",\n                    \"type\": \"Button\",\n                    \"id\": \"cfc73f02-ddaf-448f-8cec-67ff6792ea6f\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Pause\",\n                    \"type\": \"Button\",\n                    \"id\": \"d536f979-7840-4ebf-aa7d-eb0d9bec8654\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"d9178ce4-7e43-4f37-9848-2ba13eca632b\",\n                    \"path\": \"<Gamepad>/leftStick\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Horizontal\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"7c0f06b2-89fe-47a5-acdd-7cd0097875e3\",\n                    \"path\": \"<Gamepad>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Submit\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"b6c0385e-36f5-4d12-bffa-7e3bf09303d5\",\n                    \"path\": \"<Gamepad>/start\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Pause\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        }\n    ],\n    \"controlSchemes\": []\n}");
		m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
		m_Gameplay_MoveP1 = m_Gameplay.FindAction("MoveP1", throwIfNotFound: true);
		m_Gameplay_MoveP2 = m_Gameplay.FindAction("MoveP2", throwIfNotFound: true);
		m_Gameplay_MoveP3 = m_Gameplay.FindAction("MoveP3", throwIfNotFound: true);
		m_Gameplay_UpP1 = m_Gameplay.FindAction("UpP1", throwIfNotFound: true);
		m_Gameplay_UpP2 = m_Gameplay.FindAction("UpP2", throwIfNotFound: true);
		m_Gameplay_UpP3 = m_Gameplay.FindAction("UpP3", throwIfNotFound: true);
		m_Gameplay_DownP1 = m_Gameplay.FindAction("DownP1", throwIfNotFound: true);
		m_Gameplay_DownP2 = m_Gameplay.FindAction("DownP2", throwIfNotFound: true);
		m_Gameplay_DownP3 = m_Gameplay.FindAction("DownP3", throwIfNotFound: true);
		m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
		m_Menus = asset.FindActionMap("Menus", throwIfNotFound: true);
		m_Menus_Horizontal = m_Menus.FindAction("Horizontal", throwIfNotFound: true);
		m_Menus_Submit = m_Menus.FindAction("Submit", throwIfNotFound: true);
		m_Menus_Pause = m_Menus.FindAction("Pause", throwIfNotFound: true);
	}

	~GameControls()
	{
	}

	public void Dispose()
	{
		UnityEngine.Object.Destroy(asset);
	}

	public bool Contains(InputAction action)
	{
		return asset.Contains(action);
	}

	public IEnumerator<InputAction> GetEnumerator()
	{
		return asset.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Enable()
	{
		asset.Enable();
	}

	public void Disable()
	{
		asset.Disable();
	}

	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return asset.FindBinding(bindingMask, out action);
	}
}
