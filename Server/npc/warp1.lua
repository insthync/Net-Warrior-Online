if (page == "none") then
	mes("warp01-00");
	menu("warp01-01","Go.","none","Nothing.");
end
if (page == "warp01-01") then
	if (hasregval("httpguard")) then
		warp(2,8,8);
		mes("warp01-01");
		menu("none","Thank you!!.");
	else
		mes("warp01-02");
		menu("none","OK.");
	end
end