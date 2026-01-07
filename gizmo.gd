#@tool
#class_name Gizmo
#extends Resource
#
##@export_enum("Item", "Spell") var editor_gizmo_template: String:
	##set(v):
		##match v:
			##"Item":
				##var c := components.duplicate()
				##c.insert(0, DisplayComponent.new())
				##components = c
#
## Since StringNames are references counted this can just stay here
#@export var id: StringName = &"":
	#set(v): 
		## runtime-readonly
		#if Engine.is_editor_hint() || id == &"":
			#id = v
#
#@export var components: Array[Component] = []:
	#set(v):
		#if not v.any(func(e: Component) -> bool: return e is DisplayComponent):
			#v.insert(0, DisplayComponent.new())
		#
		#components = v
		#for cmp: Component in components:
			#if not cmp.LazyState.is_null():
				#states[cmp] = cmp.LazyState.call()
		#
		## Prevent arr.append()
		#if not components.is_empty():
			#components.make_read_only()
#
#var states: Dictionary[Component, Component.State] = {}
#
#
#func on_use() -> void:
	#for cmp: Component in components:
		#if cmp in states:
			#var state: Component.State = states[cmp]
			#cmp._on_use(state)
#
#func get_tool() -> ToolComponent:
	#for cmp: Component in components:
		#if cmp is ToolComponent:
			#return cmp
	#return null
#
#func get_tool_state() -> ToolComponent.ToolState:
	#for cmp: Component in states:
		#if cmp is ToolComponent:
			#return states[cmp]
	#return null
#
#
#func clone() -> Gizmo:
	#var gizmo := Gizmo.new()
	#gizmo.id = id
	#gizmo.components = components
	#gizmo.states = states.duplicate()
	#
	#return gizmo
