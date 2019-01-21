obs = obslua
json = require "json"

function script_description()
	return "CS:GO Kill \r\nBy AiSatan"
end

filePath = "C:\\Users\\aisat\\Documents\\Visual Studio 2017\\Projects\\CSGOToTwitch\\CSStatsForNerf\\bin\\Debug\\netcoreapp2.1\\events_pipe.txt"

sceneSource = obs.obs_frontend_get_current_scene();
scene = obs.obs_scene_from_source(sceneSource);

groupItem = obs.obs_scene_find_source(scene, "Gifs")
group = obs.obs_sceneitem_group_get_scene(groupItem)

JohnGame = obs.obs_scene_find_source(group, "JohnGame.gif")
dick_shot = obs.obs_scene_find_source(group, "dick_shot.gif")
niger = obs.obs_scene_find_source(group, "niger.gif")
self5 = obs.obs_scene_find_source(group, "self5.gif")
par_wow = obs.obs_scene_find_source(group, "par_wow.gif")

surprise = obs.obs_scene_find_source(group, "surprise.gif")
barney_head = obs.obs_scene_find_source(group, "barney_head.gif")
got_dance = obs.obs_scene_find_source(group, "got_dance.gif")
HAce = obs.obs_scene_find_source(group, "HAce.gif")
HAce = obs.obs_scene_find_source(group, "HAce.gif")

devil = obs.obs_scene_find_source(group, "devil.gif")
ted_wow = obs.obs_scene_find_source(group, "ted_wow.gif")
drop_on_street = obs.obs_scene_find_source(group, "drop_on_street.gif")

Bomb_exploded = obs.obs_scene_find_source(group, "Bomb_exploded.gif")
Groot_defuse = obs.obs_scene_find_source(group, "Groot_defuse.gif")
par_what = obs.obs_scene_find_source(group, "par_what.gif")

shining_scared = obs.obs_scene_find_source(group, "shining_scared.gif")

got_beggin = obs.obs_scene_find_source(group, "got_beggin.gif")

crap = obs.obs_scene_find_source(group, "crap.gif")

par_wow = obs.obs_scene_find_source(group, "par_wow.gif")
no_pls_no = obs.obs_scene_find_source(group, "no_pls_no.gif")
em_what = obs.obs_scene_find_source(group, "em_what.gif")

likeaboss_shoot = obs.obs_scene_find_source(group, "likeaboss_shoot.gif")
par_dance = obs.obs_scene_find_source(group, "par_dance.gif")
par_dance_move = obs.obs_scene_find_source(group, "par_dance_move.gif")
god_five = obs.obs_scene_find_source(group, "god_five.gif")

got_tm_kill = obs.obs_scene_find_source(group, "got_tm_kill.gif")
got_useless_tm = obs.obs_scene_find_source(group, "got_useless_tm.gif")
nigers_wow = obs.obs_scene_find_source(group, "nigers_wow.gif")



function file_exists(name)
   local f=io.open(name,"r")
   if f~=nil then io.close(f) return true else return false end
end

local function read_file()
	if not file_exists(filePath) then
		return 
	end
    local file = io.open(filePath, "rb") -- r read mode and b binary mode
    if not file then return nil end
    local content = file:read "*a" -- *a or *all reads the whole file
    file:close()
    return content
end

function SetAllInvisible()
	obs.obs_sceneitem_set_visible(JohnGame, false)
	obs.obs_sceneitem_set_visible(dick_shot, false)
	obs.obs_sceneitem_set_visible(niger, false)
	obs.obs_sceneitem_set_visible(self5, false)
	obs.obs_sceneitem_set_visible(par_wow, false)

	obs.obs_sceneitem_set_visible(surprise, false)
	obs.obs_sceneitem_set_visible(barney_head, false)
	obs.obs_sceneitem_set_visible(got_dance, false)
	obs.obs_sceneitem_set_visible(HAce, false)
	obs.obs_sceneitem_set_visible(HAce, false)

	obs.obs_sceneitem_set_visible(devil, false)
	obs.obs_sceneitem_set_visible(ted_wow, false)
	obs.obs_sceneitem_set_visible(drop_on_street, false)

	obs.obs_sceneitem_set_visible(Bomb_exploded, false)
	obs.obs_sceneitem_set_visible(Groot_defuse, false)
	obs.obs_sceneitem_set_visible(par_what, false)

	obs.obs_sceneitem_set_visible(shining_scared, false)

	obs.obs_sceneitem_set_visible(got_beggin, false)

	obs.obs_sceneitem_set_visible(crap, false)

	obs.obs_sceneitem_set_visible(par_wow, false)
	obs.obs_sceneitem_set_visible(no_pls_no, false)
	obs.obs_sceneitem_set_visible(em_what, false)

	obs.obs_sceneitem_set_visible(likeaboss_shoot, false)
	obs.obs_sceneitem_set_visible(par_dance, false)
	obs.obs_sceneitem_set_visible(par_dance_move, false)
	obs.obs_sceneitem_set_visible(god_five, false)

	obs.obs_sceneitem_set_visible(got_tm_kill, false)
	obs.obs_sceneitem_set_visible(got_useless_tm, false)
	obs.obs_sceneitem_set_visible(nigers_wow, false)
end

function changeVisibility()

	SetAllInvisible()

	res = read_file(filePath)
	
	events = json.decode(res)
	for i,v in ipairs(events) do
		if v == "Kill" then
			obs.obs_sceneitem_set_visible(JohnGame, true)
		elseif v == "DKill" then
			obs.obs_sceneitem_set_visible(dick_shot, true)
		elseif v == "TKill" then
			obs.obs_sceneitem_set_visible(niger, true)
		elseif v == "FKill" then
			obs.obs_sceneitem_set_visible(self5, true)
		elseif v == "Ace" then
			obs.obs_sceneitem_set_visible(par_wow, true)
		else 
		end

		if v == "HKill" then
			obs.obs_sceneitem_set_visible(surprise, true)
		elseif v == "HDKill" then
			obs.obs_sceneitem_set_visible(barney_head, true)
		elseif v == "HTKill" then
			obs.obs_sceneitem_set_visible(nigers_wow, true)
		elseif v == "HFKill" then
			obs.obs_sceneitem_set_visible(HAce, true)
		elseif v == "HAce" then
			obs.obs_sceneitem_set_visible(HAce, true)
		else 
		end

		if v == "TExploded" then
			obs.obs_sceneitem_set_visible(devil, true)
		elseif v == "TDefused" then
			obs.obs_sceneitem_set_visible(ted_wow, true)
		elseif v == "TPlanted" then
			obs.obs_sceneitem_set_visible(drop_on_street, true)
		else 
		end

		if v == "CTExploded" then
			obs.obs_sceneitem_set_visible(Bomb_exploded, true)
		elseif v == "CTDefused" then
			obs.obs_sceneitem_set_visible(Groot_defuse, true)
		elseif v == "CTPlanted" then
			obs.obs_sceneitem_set_visible(par_what, true)
		else 
		end

		if v == "OneHp" then
			obs.obs_sceneitem_set_visible(shining_scared, true)
		else 
		end
		
		if v == "RoundStart" then
			obs.obs_sceneitem_set_visible(got_beggin, true)
		else
		end	

		if v == "WarmUp" then
			obs.obs_sceneitem_set_visible(crap, true)
		else 
		end

		if v == "Win" then
			obs.obs_sceneitem_set_visible(par_wow, true)
		elseif v == "Lose" then
			obs.obs_sceneitem_set_visible(no_pls_no, true)
		elseif v == "Draft" then
			obs.obs_sceneitem_set_visible(em_what, true)
		else 
		end

		if v == "TenKills" then
			obs.obs_sceneitem_set_visible(likeaboss_shoot, true)
		elseif v == "TwentyKills" then
			obs.obs_sceneitem_set_visible(par_dance, true)
		elseif v == "ThirtyKills" then
			obs.obs_sceneitem_set_visible(par_dance_move, true)
		elseif v == "FortyKills" then
			obs.obs_sceneitem_set_visible(god_five, true)
		else 
		end

		if v == "UselessScore" then
			obs.obs_sceneitem_set_visible(got_tm_kill, true)
		elseif v == "UselessKD" then
			obs.obs_sceneitem_set_visible(got_useless_tm, true)
		elseif v == "GodKD" then
			obs.obs_sceneitem_set_visible(got_dance, true)
		else 
		end

	end
end



obs.timer_add(changeVisibility, 100)
