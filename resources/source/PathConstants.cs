using Godot;
using System;

public static class PathConstants
{
	//public const string PLAYER_CONTAINER_PATH = "res://resources/prefabs/units/player_units/player_unit_container.tscn";
	//public const string ENEMY_CONTAINER_PATH = "res://resources/prefabs/units/enemy_units/enemy_unit_container.tscn";
	public const string CONTAINER_UNIT_PLAYER = "res://resources/prefabs/units/container_unit_player.tscn";
	public const string CONTAINER_UNIT_ENEMY = "res://resources/prefabs/units/container_unit_enemy.tscn";


	public const string UNIT_1_PLAYER_DELTA = "res://resources/prefabs/units/player_units/unit_delta/unit_player_delta.tscn";
	public const string UNIT_2_PLAYER_IOTA = "res://resources/prefabs/units/player_units/unit_iota/unit_player_iota.tscn";
	public const string UNIT_3_PLAYER_EPSILON = "res://resources/prefabs/units/player_units/unit_epsilon/unit_player_epsilon.tscn";
	public const string UNIT_4_PLAYER_MU = "res://resources/prefabs/units/player_units/unit_mu/unit_player_mu.tscn";

	public static readonly string[] PLAYER_UNITS = { UNIT_1_PLAYER_DELTA, UNIT_2_PLAYER_IOTA, UNIT_3_PLAYER_EPSILON, UNIT_4_PLAYER_MU};

	public const string UNIT_ENEMY_T1_PLACEHOLDER = "res://resources/prefabs/units/enemy_units/unit_enemy_T1_placeholder.tscn";
	public const string UNIT_ENEMY_T2_PLACEHOLDER = "res://resources/prefabs/units/enemy_units/unit_enemy_T2_placeholder.tscn";
	public const string UNIT_ENEMY_T3_PLACEHOLDER = "res://resources/prefabs/units/enemy_units/unit_enemy_T3_placeholder.tscn";

	public const string LEVEL_TREE_PLACEHOLDER = "res://resources/prefabs/combat_level/level_tree_placeholder.tscn";
	public const string LEVEL_NODE = "res://resources/prefabs/combat_level/level_node.tscn";
	public const string PLACEHOLDER_LEVEL = "res://resources/prefabs/combat_level/level_placeholder.tscn";

	public const string STATUS_EFFECT_UI_CONTAINER_GLOBAL = "res://resources/prefabs/status_effects/UI/status_effect_ui_container.tscn";
}
