using TestersManager.Core.Domain.Entities;
using TestersManager.Core.Enums;

namespace TestersManager.Core.DTO;

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
    public string? DevStream { get; set; }
    public string? Position { get; set; }
    public string? Skills { get; set; }
    public Guid? DevStreamId { get; set; }
    public int? MonthsOfWorkExperience { get; set; }
    public int? Age { get; set; }


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
                   && MonthsOfWorkExperience == testerResponse.MonthsOfWorkExperience;
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
               Position == other.Position && MonthsOfWorkExperience == other.MonthsOfWorkExperience;
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

    public override string ToString()
    {
        return
            $"{TesterId} {TesterName} {Email} {Gender} {BirthDate?.ToString()} " +
            $"{DevStreamId} {DevStream} {Position} ";
    }

    public TesterUpdateRequest ToTesterUpdateRequest()
    {
        return new TesterUpdateRequest
        {
            TesterId = TesterId,
            TesterName = TesterName,
            Email = Email,
            Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
            BirthDate = BirthDate,
            DevStreamId = DevStreamId,
            Position = Position,
            MonthsOfWorkExperience = MonthsOfWorkExperience,
            Skills = Skills
        };
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
        DevStreamId = tester.DevStreamId,
        Position = tester.Position,
        MonthsOfWorkExperience = tester.MonthsOfWorkExperience,
        Skills = tester.Skills,
        DevStream = tester.DevStream?.DevStreamName,
    };
}