//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/UI/Player Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Controls"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""682c5233-b703-4213-a12a-812b85a4492a"",
            ""actions"": [
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""f9ef4062-4cb9-4819-80ad-ab739f49a541"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""691fc475-bf8a-431a-b41a-6b391c866a3c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""b9561f1c-918f-4528-b11c-ae6dfe56bba4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""72f6fcf1-00fe-4f65-9745-cc07d823a3ca"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CursorPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9cd72c31-be3c-499c-a552-7c49228f05be"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""5639a152-b408-46de-aa8d-f4772c68d013"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""909f5a50-3ca4-465e-ac47-c475509b8cc0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3633e8dd-c257-4b82-a79c-1ea055b7bca2"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68f7c2b2-1133-4d5b-a2be-cb2286a18a53"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4de153e0-6da7-42d7-8d71-440191a3f8a7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66674172-1387-41b0-b922-e01ec2c84611"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9e81943-085e-4e6c-868f-63e36f0ebb46"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ebf4fda-a389-4226-bbd6-a987ffb6ec0e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""52aabf5c-642f-42fd-9c4d-0741049fb23a"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dca56550-5005-4929-a662-e1b69fb113f4"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7531fcb-6d13-4633-a23a-7e89b3bddef1"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""dfbea67b-a596-42d5-949c-d3c5b57230fe"",
            ""actions"": [
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""6956f045-7acc-4b50-9a03-445ee7769ed5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CursorPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""842a4243-f6a7-42eb-8919-fac01cdac35a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Unpause"",
                    ""type"": ""Button"",
                    ""id"": ""78ffb3b1-dc9d-42ca-93f9-f124d6c0b9ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""02fc08ef-464b-48fa-999a-1ea068449631"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bacf54c9-bfa4-458c-a998-72b55e16b76c"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba8aea25-6a4b-4333-bbe9-62fbb82c1766"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4cae789b-980e-4c31-a39d-128771ad3f56"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75cf0f9a-048a-438c-b15c-7c9cf1b427a2"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""747200ee-b129-4948-b271-dae7170a3db1"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_Interact = m_Game.FindAction("Interact", throwIfNotFound: true);
        m_Game_Scroll = m_Game.FindAction("Scroll", throwIfNotFound: true);
        m_Game_Select = m_Game.FindAction("Select", throwIfNotFound: true);
        m_Game_Aim = m_Game.FindAction("Aim", throwIfNotFound: true);
        m_Game_CursorPosition = m_Game.FindAction("CursorPosition", throwIfNotFound: true);
        m_Game_Pause = m_Game.FindAction("Pause", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Interact = m_Menu.FindAction("Interact", throwIfNotFound: true);
        m_Menu_CursorPosition = m_Menu.FindAction("CursorPosition", throwIfNotFound: true);
        m_Menu_Unpause = m_Menu.FindAction("Unpause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_Interact;
    private readonly InputAction m_Game_Scroll;
    private readonly InputAction m_Game_Select;
    private readonly InputAction m_Game_Aim;
    private readonly InputAction m_Game_CursorPosition;
    private readonly InputAction m_Game_Pause;
    public struct GameActions
    {
        private @PlayerControls m_Wrapper;
        public GameActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interact => m_Wrapper.m_Game_Interact;
        public InputAction @Scroll => m_Wrapper.m_Game_Scroll;
        public InputAction @Select => m_Wrapper.m_Game_Select;
        public InputAction @Aim => m_Wrapper.m_Game_Aim;
        public InputAction @CursorPosition => m_Wrapper.m_Game_CursorPosition;
        public InputAction @Pause => m_Wrapper.m_Game_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @Interact.started -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @Scroll.started -= m_Wrapper.m_GameActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnScroll;
                @Select.started -= m_Wrapper.m_GameActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnSelect;
                @Aim.started -= m_Wrapper.m_GameActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnAim;
                @CursorPosition.started -= m_Wrapper.m_GameActionsCallbackInterface.OnCursorPosition;
                @CursorPosition.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnCursorPosition;
                @CursorPosition.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnCursorPosition;
                @Pause.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @CursorPosition.started += instance.OnCursorPosition;
                @CursorPosition.performed += instance.OnCursorPosition;
                @CursorPosition.canceled += instance.OnCursorPosition;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public GameActions @Game => new GameActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Interact;
    private readonly InputAction m_Menu_CursorPosition;
    private readonly InputAction m_Menu_Unpause;
    public struct MenuActions
    {
        private @PlayerControls m_Wrapper;
        public MenuActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interact => m_Wrapper.m_Menu_Interact;
        public InputAction @CursorPosition => m_Wrapper.m_Menu_CursorPosition;
        public InputAction @Unpause => m_Wrapper.m_Menu_Unpause;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Interact.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnInteract;
                @CursorPosition.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnCursorPosition;
                @CursorPosition.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnCursorPosition;
                @CursorPosition.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnCursorPosition;
                @Unpause.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnUnpause;
                @Unpause.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnUnpause;
                @Unpause.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnUnpause;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @CursorPosition.started += instance.OnCursorPosition;
                @CursorPosition.performed += instance.OnCursorPosition;
                @CursorPosition.canceled += instance.OnCursorPosition;
                @Unpause.started += instance.OnUnpause;
                @Unpause.performed += instance.OnUnpause;
                @Unpause.canceled += instance.OnUnpause;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    public interface IGameActions
    {
        void OnInteract(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnCursorPosition(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnInteract(InputAction.CallbackContext context);
        void OnCursorPosition(InputAction.CallbackContext context);
        void OnUnpause(InputAction.CallbackContext context);
    }
}
