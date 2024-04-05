//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_Source/InputSystem/PlayerInputAction.inputactions
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

public partial class @PlayerInputAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""BuilderController"",
            ""id"": ""2942b663-e167-43e0-873f-ee24b7ccf45b"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""48879b9a-87a5-4b19-b455-f72ae825ff08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""2e28542a-2221-4b4c-9bf4-7beb3ed31ba0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""5bf2ac9b-6b18-45fe-8769-8205d619b4fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""cacde52d-e3a0-4aa4-891a-990b0e13a8f3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""0a7a2ff1-5880-4227-8450-558978277f72"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""90c0a2f6-ece5-447b-a600-a195bb8cb776"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""44738f0b-1934-4e0f-9b5c-83bbaa6d8804"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d0eb7c0d-1fa5-4c77-9b88-21be2c2da10c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8546b1aa-3950-4592-85c6-5a773f9ad490"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9edc6053-4c92-4c00-a8e8-3cc5645d8d54"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1c6b4ce0-f732-4840-8ced-b895bfbd9dae"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""9b8717a1-0623-4b35-ba0a-c7a17f8c27fa"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": ""Normalize(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""2808aa8c-dacf-4086-9e38-8c958cd80754"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b6290908-5e9d-469c-82ce-d5889a570087"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""FPSController"",
            ""id"": ""557a4efe-bedd-464b-9a4c-121c454409c2"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""f997e49e-6b3e-4418-9dbe-0aa3d91999f3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""e9e1558b-ef57-4b39-932e-f4f43ffc97e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""e57e9b73-e177-4b53-9458-431d2df291e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Button"",
                    ""id"": ""65b4952d-6b52-4401-9d6a-9ab0b29decf3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchToLastWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""0ac1d80d-4b9e-4b6c-ac69-be616d22e62d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSwitch"",
                    ""type"": ""Button"",
                    ""id"": ""463e2561-9737-49f7-b7e7-afc954803b77"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3216a34f-3efe-450d-ba38-6d7168dde656"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""05bf6855-ad98-4fb8-a876-1f2f980c794c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""df62fe77-9893-4934-96c8-52d018f266ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""592bb9c0-6661-4d90-abc6-d53c12602165"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Grenade"",
                    ""type"": ""Button"",
                    ""id"": ""4ed356ae-398a-4bda-9f10-8c24c3438866"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector - Keyboard"",
                    ""id"": ""44780d13-26bc-41a0-9a1c-7e44d0d636e8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a3e756db-30c5-4648-a658-e3336ddf6c39"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0eb1058f-8a35-4855-83c2-76342ece0ff1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7f62c0cc-a137-4c28-9b5e-ce1a8dfd2d7a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3a32211e-ed3c-49de-90a1-e759ef75d6a1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1d3feae8-0625-4806-b833-6c304d455e45"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""545c35ec-1ded-4c09-8dab-e2ca6439fc9a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""678e7a3b-9258-4b82-84a1-fb954f59fb08"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""642602e7-65e1-41a6-8a72-a3f041187cdc"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""ca20be22-840d-438f-9ff7-d7b237fda8f2"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""536f65c0-567f-47e7-8c07-08913c64105b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": ""Scale"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""WeaponSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6af958dc-a8c9-46dc-87b1-b4bf058003da"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=2)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""WeaponSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd956c13-908b-4e81-a338-28ceb2a00a8f"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=3)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""WeaponSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53307657-259f-4ffe-bbe2-cd0ac4e840ef"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=4)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""WeaponSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75182c31-e22d-4f9f-a164-a4ab8084de8f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwitchToLastWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""218582ab-4f9b-4d65-95cd-632501df1054"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87b58fbe-d8c6-4e11-b261-770a14877032"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34127494-33e8-4f37-9c15-8bdb773f5921"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9217eedf-c43f-4ef5-a8db-bc6c0ff483de"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ac49a3c-ad30-452e-ba16-47ccb058ca80"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Grenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // BuilderController
        m_BuilderController = asset.FindActionMap("BuilderController", throwIfNotFound: true);
        m_BuilderController_Rotate = m_BuilderController.FindAction("Rotate", throwIfNotFound: true);
        m_BuilderController_Movement = m_BuilderController.FindAction("Movement", throwIfNotFound: true);
        m_BuilderController_Zoom = m_BuilderController.FindAction("Zoom", throwIfNotFound: true);
        // FPSController
        m_FPSController = asset.FindActionMap("FPSController", throwIfNotFound: true);
        m_FPSController_Movement = m_FPSController.FindAction("Movement", throwIfNotFound: true);
        m_FPSController_Fire = m_FPSController.FindAction("Fire", throwIfNotFound: true);
        m_FPSController_Aim = m_FPSController.FindAction("Aim", throwIfNotFound: true);
        m_FPSController_Scroll = m_FPSController.FindAction("Scroll", throwIfNotFound: true);
        m_FPSController_SwitchToLastWeapon = m_FPSController.FindAction("SwitchToLastWeapon", throwIfNotFound: true);
        m_FPSController_WeaponSwitch = m_FPSController.FindAction("WeaponSwitch", throwIfNotFound: true);
        m_FPSController_Look = m_FPSController.FindAction("Look", throwIfNotFound: true);
        m_FPSController_Jump = m_FPSController.FindAction("Jump", throwIfNotFound: true);
        m_FPSController_Dash = m_FPSController.FindAction("Dash", throwIfNotFound: true);
        m_FPSController_Reload = m_FPSController.FindAction("Reload", throwIfNotFound: true);
        m_FPSController_Grenade = m_FPSController.FindAction("Grenade", throwIfNotFound: true);
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

    // BuilderController
    private readonly InputActionMap m_BuilderController;
    private List<IBuilderControllerActions> m_BuilderControllerActionsCallbackInterfaces = new List<IBuilderControllerActions>();
    private readonly InputAction m_BuilderController_Rotate;
    private readonly InputAction m_BuilderController_Movement;
    private readonly InputAction m_BuilderController_Zoom;
    public struct BuilderControllerActions
    {
        private @PlayerInputAction m_Wrapper;
        public BuilderControllerActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_BuilderController_Rotate;
        public InputAction @Movement => m_Wrapper.m_BuilderController_Movement;
        public InputAction @Zoom => m_Wrapper.m_BuilderController_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_BuilderController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BuilderControllerActions set) { return set.Get(); }
        public void AddCallbacks(IBuilderControllerActions instance)
        {
            if (instance == null || m_Wrapper.m_BuilderControllerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BuilderControllerActionsCallbackInterfaces.Add(instance);
            @Rotate.started += instance.OnRotate;
            @Rotate.performed += instance.OnRotate;
            @Rotate.canceled += instance.OnRotate;
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
        }

        private void UnregisterCallbacks(IBuilderControllerActions instance)
        {
            @Rotate.started -= instance.OnRotate;
            @Rotate.performed -= instance.OnRotate;
            @Rotate.canceled -= instance.OnRotate;
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
        }

        public void RemoveCallbacks(IBuilderControllerActions instance)
        {
            if (m_Wrapper.m_BuilderControllerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBuilderControllerActions instance)
        {
            foreach (var item in m_Wrapper.m_BuilderControllerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BuilderControllerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BuilderControllerActions @BuilderController => new BuilderControllerActions(this);

    // FPSController
    private readonly InputActionMap m_FPSController;
    private List<IFPSControllerActions> m_FPSControllerActionsCallbackInterfaces = new List<IFPSControllerActions>();
    private readonly InputAction m_FPSController_Movement;
    private readonly InputAction m_FPSController_Fire;
    private readonly InputAction m_FPSController_Aim;
    private readonly InputAction m_FPSController_Scroll;
    private readonly InputAction m_FPSController_SwitchToLastWeapon;
    private readonly InputAction m_FPSController_WeaponSwitch;
    private readonly InputAction m_FPSController_Look;
    private readonly InputAction m_FPSController_Jump;
    private readonly InputAction m_FPSController_Dash;
    private readonly InputAction m_FPSController_Reload;
    private readonly InputAction m_FPSController_Grenade;
    public struct FPSControllerActions
    {
        private @PlayerInputAction m_Wrapper;
        public FPSControllerActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_FPSController_Movement;
        public InputAction @Fire => m_Wrapper.m_FPSController_Fire;
        public InputAction @Aim => m_Wrapper.m_FPSController_Aim;
        public InputAction @Scroll => m_Wrapper.m_FPSController_Scroll;
        public InputAction @SwitchToLastWeapon => m_Wrapper.m_FPSController_SwitchToLastWeapon;
        public InputAction @WeaponSwitch => m_Wrapper.m_FPSController_WeaponSwitch;
        public InputAction @Look => m_Wrapper.m_FPSController_Look;
        public InputAction @Jump => m_Wrapper.m_FPSController_Jump;
        public InputAction @Dash => m_Wrapper.m_FPSController_Dash;
        public InputAction @Reload => m_Wrapper.m_FPSController_Reload;
        public InputAction @Grenade => m_Wrapper.m_FPSController_Grenade;
        public InputActionMap Get() { return m_Wrapper.m_FPSController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FPSControllerActions set) { return set.Get(); }
        public void AddCallbacks(IFPSControllerActions instance)
        {
            if (instance == null || m_Wrapper.m_FPSControllerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_FPSControllerActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
            @Scroll.started += instance.OnScroll;
            @Scroll.performed += instance.OnScroll;
            @Scroll.canceled += instance.OnScroll;
            @SwitchToLastWeapon.started += instance.OnSwitchToLastWeapon;
            @SwitchToLastWeapon.performed += instance.OnSwitchToLastWeapon;
            @SwitchToLastWeapon.canceled += instance.OnSwitchToLastWeapon;
            @WeaponSwitch.started += instance.OnWeaponSwitch;
            @WeaponSwitch.performed += instance.OnWeaponSwitch;
            @WeaponSwitch.canceled += instance.OnWeaponSwitch;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @Grenade.started += instance.OnGrenade;
            @Grenade.performed += instance.OnGrenade;
            @Grenade.canceled += instance.OnGrenade;
        }

        private void UnregisterCallbacks(IFPSControllerActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
            @Scroll.started -= instance.OnScroll;
            @Scroll.performed -= instance.OnScroll;
            @Scroll.canceled -= instance.OnScroll;
            @SwitchToLastWeapon.started -= instance.OnSwitchToLastWeapon;
            @SwitchToLastWeapon.performed -= instance.OnSwitchToLastWeapon;
            @SwitchToLastWeapon.canceled -= instance.OnSwitchToLastWeapon;
            @WeaponSwitch.started -= instance.OnWeaponSwitch;
            @WeaponSwitch.performed -= instance.OnWeaponSwitch;
            @WeaponSwitch.canceled -= instance.OnWeaponSwitch;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @Grenade.started -= instance.OnGrenade;
            @Grenade.performed -= instance.OnGrenade;
            @Grenade.canceled -= instance.OnGrenade;
        }

        public void RemoveCallbacks(IFPSControllerActions instance)
        {
            if (m_Wrapper.m_FPSControllerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IFPSControllerActions instance)
        {
            foreach (var item in m_Wrapper.m_FPSControllerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_FPSControllerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public FPSControllerActions @FPSController => new FPSControllerActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface IBuilderControllerActions
    {
        void OnRotate(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
    public interface IFPSControllerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnSwitchToLastWeapon(InputAction.CallbackContext context);
        void OnWeaponSwitch(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnGrenade(InputAction.CallbackContext context);
    }
}
