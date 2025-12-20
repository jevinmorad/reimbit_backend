using RandomString4Net;

namespace Common.Services;

public class PasswordGenerator
{
    public string GeneratePassword(int length)
    {
        return RandomString.GetString(Types.ALPHANUMERIC_MIXEDCASE_WITH_SYMBOLS, length);
    }
}
