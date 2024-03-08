using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
///  DTO class to use as a return type of TesterService methods
/// </summary>
public class TesterResponse : IEquatable<TesterResponse>
{
    public Guid TesterId { get; set; }
    public string? TesterName { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age { get; set; }
    public Guid? DevStreamId { get; set; }
    public string? DevStream { get; set; }
    public string? Position { get; set; }
    public int? MonthsOfWorkExperience { get; set; }
    public bool HasMobileDeviceExperience { get; set; }
    public string? Skills { get; set; }


    /// <summary>
    /// Compares two objects and returns true if they are equal
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        // implicit type conversion, could have use record instead
        if (obj is TesterResponse testerResponse)
        {
            return TesterId == testerResponse.TesterId
                   && TesterName == testerResponse.TesterName
                   && Email == testerResponse.Email
                   && Gender == testerResponse.Gender
                   && BirthDate == testerResponse.BirthDate
                   && DevStreamId == testerResponse.DevStreamId
                   && DevStream == testerResponse.DevStream
                   && Position == testerResponse.Position
                   && MonthsOfWorkExperience == testerResponse.MonthsOfWorkExperience
                   && HasMobileDeviceExperience == testerResponse.HasMobileDeviceExperience;
        }

        return false;
    }

    public bool Equals(TesterResponse? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return TesterId.Equals(other.TesterId) && TesterName == other.TesterName && Email == other.Email &&
               Gender == other.Gender && BirthDate.Equals(other.BirthDate) &&
               Nullable.Equals(DevStreamId, other.DevStreamId) && DevStream == other.DevStream &&
               Position == other.Position && MonthsOfWorkExperience == other.MonthsOfWorkExperience &&
               HasMobileDeviceExperience == other.HasMobileDeviceExperience && Equals(Skills, other.Skills);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(TesterId);
        hashCode.Add(TesterName);
        hashCode.Add(Email);
        hashCode.Add(Gender);
        hashCode.Add(BirthDate);
        hashCode.Add(DevStreamId);
        hashCode.Add(DevStream);
        hashCode.Add(Position);
        hashCode.Add(MonthsOfWorkExperience);
        hashCode.Add(HasMobileDeviceExperience);
        hashCode.Add(Skills);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(TesterResponse? left, TesterResponse? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TesterResponse? left, TesterResponse? right)
    {
        return !Equals(left, right);
    }
}

public static class TesterExtensions
{
    /// <summary>
    ///  Converts Tester to TesterResponse DTO
    /// </summary>
    /// <param name="tester"></param>
    /// <returns></returns>
    public static TesterResponse ToTesterResponse(this Tester tester) => new()

    {
        TesterId = tester.TesterId,
        TesterName = tester.TesterName,
        Email = tester.Email,
        Gender = tester.Gender,
        BirthDate = tester.BirthDate,
        Age = tester.BirthDate != null ? DateTime.Now.Year - tester.BirthDate.Value.Year : null,
        DevStream = tester.DevStream?.DevStreamName,
        DevStreamId = tester.DevStreamId,
        Position = tester.Position,
        MonthsOfWorkExperience = tester.MonthsOfWorkExperience,
        HasMobileDeviceExperience = tester.HasMobileDeviceExperience,
        Skills = tester.Skills,
    };
}