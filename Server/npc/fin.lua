if (page == "none") then
	if (hasregval("main") == true) then
		mes("npc00-02");
		menu("none","End.");
	else
		mes("npc01-00");
		menu("npc01-01","Next.","none","End.");
	end
end
if (page == "npc01-01") then
	mes("npc01-01");
	menu("npc01-02","Next.","none","End.");
end
if (page == "npc01-02") then
	mes("npc01-02");
	menu("npc01-03","Next.","none","End.");
end
if (page == "npc01-03") then
	mes("npc01-03");
	menu("npc01-04","Next.","none","End.");
end
if (page == "npc01-04") then
	mes("npc01-04");
	menu("npc01-05","Next.","none","End.");
end
if (page == "npc01-05") then
	mes("npc01-05");
	menu("npc01-06","Next.","none","End.");
end
if (page == "npc01-06") then
	mes("npc01-06");
	menu("npc01-07","Next.","none","End.");
end
if (page == "npc01-07") then
	setregval("main","1");
	mes("npc01-07");
	menu("none","End.");
end