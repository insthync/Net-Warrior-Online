if (page == "none") then
	mes("warp04-00");
	menu("warp04-01","Go.","none","Nothing.");
end
if (page == "warp04-01") then
	if (hasregval("dbguard")) then
		warp(5,248,8);
		mes("warp04-01");
		menu("none","Thank you!!.");
	else
		mes("warp04-02");
		menu("none","OK.");
	end
end