if (page == "none") then
	mes("in02-00");
	menu("in02-01","Data_Transmission_Definition","in02-02","Type_of_Data_Definition","none","Nothing.");
end
if (page == "in02-00") then
	mes("in02-00");
	menu("in02-01","Data_Transmission_Definition","in02-02","Type_of_Data_Definition","none","Nothing.");
end
if (page == "in02-01") then
	mes("in02-01");
	menu("in02-00","OK");
end
if (page == "in02-02") then
	mes("in02-02");
	menu("in02-00","OK");
end