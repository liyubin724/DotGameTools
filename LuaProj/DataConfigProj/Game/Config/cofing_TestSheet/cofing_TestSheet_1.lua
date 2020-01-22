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
    1,
    2,
    3,
    4,
}
local t_4 = {
    1,
    2,
    3,
    5,
}
local t_5 = {
    [1] = 1,
    [2] = 2,
}
local cofing_TestSheet_1 = {}
cofing_TestSheet_1.__index = cofing_TestSheet_1
setmetatable(cofing_TestSheet_1,ConfigSubSheet)
cofing_TestSheet_1.ids = {1,4,5,}
cofing_TestSheet_1[1] = {
    ID = 1,
    LongValue = 1,
    FloatValue = 1,
    StringValue_Index = 1,
    TextValue = 1,
    ResValue_Index = 8,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[1].__index = cofing_TestSheet_1[1]
setmetatable(cofing_TestSheet_1[1],ConfigSheetLine)

cofing_TestSheet_1[4] = {
    ID = 4,
    StringValue_Index = 2,
    LuaValue = t_1,
    ArrayValue = t_3,
    DicValue = t_5,
}
cofing_TestSheet_1[4].__index = cofing_TestSheet_1[4]
setmetatable(cofing_TestSheet_1[4],ConfigSheetLine)

cofing_TestSheet_1[5] = {
    ID = 5,
    LongValue = 65,
    BoolValue = false,
    StringValue_Index = 2,
    ResValue_Index = 1,
    LuaValue = t_1,
    ArrayValue = t_4,
    DicValue = t_5,
}
cofing_TestSheet_1[5].__index = cofing_TestSheet_1[5]
setmetatable(cofing_TestSheet_1[5],ConfigSheetLine)

return cofing_TestSheet_1
