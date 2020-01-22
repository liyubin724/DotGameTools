require ("Dot/Config/ConfigSubSheet")
require ("Dot/Config/ConfigSheetLine")
local t_1 =function()
    return 1.0
end
local t_2 = {
    1,
    2,
    3,
}
local t_3 = {
    [1] = 1,
    [2] = 2,
}
local cofing_TestSheet_2 = {}
cofing_TestSheet_2.__index = cofing_TestSheet_2
setmetatable(cofing_TestSheet_2,ConfigSubSheet)
cofing_TestSheet_2.ids = {8,9,11,}
cofing_TestSheet_2[8] = {
    ID = 8,
    FloatValue = 1.4,
    BoolValue = false,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_2[8].__index = cofing_TestSheet_2[8]
setmetatable(cofing_TestSheet_2[8],ConfigSheetLine)

cofing_TestSheet_2[9] = {
    ID = 9,
    LongValue = 234,
    FloatValue = 11.1,
    StringValue_Index = 4,
    ResValue_Index = 10,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_2[9].__index = cofing_TestSheet_2[9]
setmetatable(cofing_TestSheet_2[9],ConfigSheetLine)

cofing_TestSheet_2[11] = {
    ID = 11,
    LongValue = 6,
    FloatValue = 1,
    StringValue_Index = 2,
    TextValue = 1,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_2[11].__index = cofing_TestSheet_2[11]
setmetatable(cofing_TestSheet_2[11],ConfigSheetLine)

return cofing_TestSheet_2
