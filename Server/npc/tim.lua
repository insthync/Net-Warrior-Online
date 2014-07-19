if (page == "none") then
	if (checkregval("main","1") == true) then
		if (hasregval("meettim") == false) then
			setregval("meettim","1");
			addexp(500);
		end
		mes("npc02-00");
		menu("npc02-01","Next.","none","End.");
	elseif (hasregval("main") == true) then
		mes("npc00-02");
		menu("none","End.");
	else
		mes("npc00-00");
		menu("none","End.");
	end
end
if (page == "npc02-01") then
	mes("npc02-01");
	menu("npc02-02","Next.","none","End.");
end
if (page == "npc02-02") then
	mes("npc02-02");
	menu("npc02-03","Next.","none","End.");
end
if (page == "npc02-03") then
	setregval("main","2");
	mes("npc02-03");
	menu("none","End.");
end