if (page == "none") then
	if (checkregval("main","3") == true) and (hasregval("killed") == false) then
		if (checklevel(3) == true) then
			setregval("killed","1");
			setregval("pop3guard","1");
			addexp(1000);
			giveitem(3);
			mes("npc03-05");
			menu("npc03-08","Next.","none","End.");
		else
			mes("npc00-01");
			menu("none","End.");
		end
	elseif (checkregval("main","3") == true) and (checkregval("killed","1") == true) then
		if (checklevel(5) == true) then
			setregval("killed","2");
			setregval("ftpguard","1");
			addexp(1500);
			giveitem(2);
			mes("npc03-06");
			menu("npc03-09","Next.","none","End.");
		else
			mes("npc00-01");
			menu("none","End.");
		end
	elseif (checkregval("main","3") == true) and (checkregval("killed","2") == true) then
		if (checklevel(7) == true) then
			setregval("killed","3");
			addexp(2250);
			giveitem(1);
			mes("npc03-07");
			menu("none","End.");
		else
			mes("npc00-01");
			menu("none","End.");
		end
	elseif (checkregval("main","2") == true) then
		if (hasregval("meetmax") == false) then
			setregval("meetmax","1");
			addexp(750);
		end
		mes("npc03-00");
		menu("npc03-01","Next.","none","End.");
	elseif (hasregval("main") == true) then
		mes("npc00-02");
		menu("none","End.");
	else
		mes("npc00-00");
		menu("none","End.");
	end
end
if (page == "npc03-01") then
	mes("npc03-01");
	menu("npc03-02","Next.","none","End.");
end
if (page == "npc03-02") then
	mes("npc03-02");
	menu("npc03-03","Next.","none","End.");
end
if (page == "npc03-03") then
	mes("npc03-03");
	menu("npc03-04","Next.","none","End.");
end
if (page == "npc03-04") then
	setregval("main","3");
	setregval("httpguard","1");
	mes("npc03-04");
	menu("none","End.");
end
if (page == "npc03-08") then
	mes("npc03-08");
	menu("none","End.");
end
if (page == "npc03-09") then
	mes("npc03-09");
	menu("none","End.");
end