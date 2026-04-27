# Certificate Grade Calculation

This document describes how certificate grades are calculated in `TrgBL`.

## Purpose
The grade is derived from a participant’s learning completion percentage for a training session. The calculation uses the configured certificate percentage ranges and maps the participant’s completion percentage to a grade.

## Data Sources
- `TrainingSettings.json` (`certificate_setting.certificate_percentage`) for grade thresholds.
- `SessionDB.Get_Session_Data_By_Trg(trainingid)` for session duration.
- `SupportBL.Learning_Report_Data(trainingid, participantid, ...)` for participant learning time.

## Database Objects
- View: `trainingplan.Vw_tp_trg_time_table` (used by `SessionDB.Get_Session_Data_By_Trg`).
- Stored procedure: `yuser.proc_yuser_participant_learning_report` (used by `SupportDB.Learning_Report_Data`).

## Calculation Flow
1. Load `certificate_percentage` ranges from `TrainingSettings.json`.
2. Load all non-deleted sessions for the training.
3. Sum durations of sessions excluding Break, Test, and Assignment types.
4. Read the participant’s total learning time from the learning report data.
5. Compute completion percentage:
   - `participant_total_learning_time / trg_session_total_learning_time * 100`
6. Find the first range where the completion percentage falls between `from` and `to`.
7. Return the associated `grade`.

## Edge Cases
- If there are no matching ranges, an empty string is returned.
- If the participant has no learning time, the percentage can be zero.
- Ensure session durations are available and total duration is not zero to avoid divide-by-zero.

## Related Method
- `TrgBL.Calculate_Certificate_grade` in `BLContext/TrgBL.cs`
