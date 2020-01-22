require ("Dot/Config/ConfigSummarySheet")
local cofing_TestSheet = {}
cofing_TestSheet.__index = cofing_TestSheet

cofing_TestSheet.depends={
    text = require ("Game/Config/cofing_Text"),
    strFieldNames = {
        "StringValue",
        "ResValue",
    },
    strValues = {
        [===[test]===],
        [===[String]===],
        [===[Value]===],
        [===[Today]===],
        [===[Doay]===],
        [===[Dla]===],
        [===[Test]===],
        [===[set]===],
        [===[path]===],
        [===[doto]===],
        [===[dodo]===],
        [===[tt]===],
        [===[doy]===],
        [===[ss]===],
    },
    defaultValues = {
        LongValue = 56,
        FloatValue = 1.1,
        BoolValue = true,
        StringValue_Index = 3,
        TextValue = 2,
        ResValue_Index = 9,
    },
}
cofing_TestSheet.subSheets = {
    {
        data = nil,
        startID = 1,
        endID = 5,
        path = "Game/Config/cofing_TestSheet/cofing_TestSheet_1",
    },
    {
        data = nil,
        startID = 8,
        endID = 11,
        path = "Game/Config/cofing_TestSheet/cofing_TestSheet_2",
    },
    {
        data = nil,
        startID = 12,
        endID = 20,
        path = "Game/Config/cofing_TestSheet/cofing_TestSheet_3",
    },
    {
        data = nil,
        startID = 31,
        endID = 54,
        path = "Game/Config/cofing_TestSheet/cofing_TestSheet_4",
    },
    {
        data = nil,
        startID = 111,
        endID = 111,
        path = "Game/Config/cofing_TestSheet/cofing_TestSheet_5",
    },
}
setmetatable(cofing_TestSheet,ConfigSummarySheet)
return cofing_TestSheet
