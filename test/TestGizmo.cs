using System.Diagnostics.CodeAnalysis;
using Godot.Collections;
using rpgcore.gizmo;
using rpgcore.gizmo.components;

namespace rpgcore.test;

using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
// ReSharper disable once UnusedType.Global
public class TestGizmo {
    private Gizmo _gizmo = null!;
    private Gizmo _dupedGizmo = null!;
    private Gizmo _differentGizmo = null!;

    [BeforeTest]
    [RequireGodotRuntime]
    public void BeforeEachTest() {
        _gizmo = new Gizmo();
        _differentGizmo = new Gizmo();
        _differentGizmo.Set(Gizmo.ComponentsExportName,
            new Array() { new DisplayComponent() { Lore = "Different lore" } });
        _gizmo.Set(Gizmo.ComponentsExportName, new Array() { new DisplayComponent() { Lore = "Normal lore" } });
        _dupedGizmo = _gizmo.Duplicate();
    }

    [TestCase]
    public void TestDuplicate() {
        AssertObject(_gizmo).IsNotSame(_dupedGizmo);
    }

    [TestCase]
    public void TestAddAndGetComponent() {
        AssertObject(_gizmo.GetComponent<DisplayComponent>()).IsNotNull();
    }

    [TestCase]
    public void TestComponentsAreDifferentForDuplicatedGizmo() {
        AssertObject(_gizmo.GetComponent<DisplayComponent>()).IsNotSame(_dupedGizmo.GetComponent<DisplayComponent>()!);
    }

    [TestCase]
    public void TestComponentShareDataIsSameForDuplicatedGizmo() {
        AssertObject(_gizmo.GetComponent<DisplayComponent>()!.DisplaySharedData)
            .IsSame(_dupedGizmo.GetComponent<DisplayComponent>()!.DisplaySharedData);
    }

    [TestCase]
    public void TestComponentsAreDifferentForDifferentGizmos() {
        AssertObject(_gizmo.GetComponent<DisplayComponent>())
            .IsNotSame(_differentGizmo.GetComponent<DisplayComponent>()!);
    }

    [TestCase]
    public void TestComponentShareDataIsSameForDifferentGizmos() {
        AssertObject(_gizmo.GetComponent<DisplayComponent>()!.DisplaySharedData)
            .IsNotSame(_differentGizmo.GetComponent<DisplayComponent>()!.DisplaySharedData);
    }

    [TestCase]
    public void TestComponentPropertyDuplication() {
        AssertString(_gizmo.GetComponent<DisplayComponent>()!.Lore)
            .IsEqual(_dupedGizmo.GetComponent<DisplayComponent>()!.Lore);
    }
}