#class_name ToolComponent
#extends Component
#
#@export var max_durability: int
#@export var haste: int
#
#func _init() -> void:
	#LazyState = func() -> ToolState:
		#var state := ToolState.new()
		#state.current_durability = max_durability
		#return state
#
#func _on_use(state: State) -> void:
	#var tool_state := state as ToolState
	#tool_state.current_durability -= 1
#
#class ToolState extends State:
	#@export var current_durability: int
