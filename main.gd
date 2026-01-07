extends Control

func _ready() -> void:
	var g1: Gizmo = load("test.tres")
	print(g1.States)
	#var pick2: Gizmo = pick.clone()
	##pick.components[0].display_name = "123"
	##print(pick, " ", pick.components[0].display_name)
	##print(pick2, " ", pick2.components[0].display_name)
	#
	#pick.components[1].max_durability = 111
	#print(pick, " ", pick.components[1].max_durability)
	#print(pick2, " ", pick2.components[1].max_durability)
	#print("\n")
	#print(pick, " ", pick.get_tool_state().current_durability)
	#print(pick2, " ", pick2.get_tool_state().current_durability)
	#pick.on_use()
	#print(pick, " ", pick.get_tool_state().current_durability)
	#print(pick2, " ", pick2.get_tool_state().current_durability)
