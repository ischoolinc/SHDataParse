## Goal

完成「刪除學習歷程成績相關資料」功能。

主要處理：

1. 依畫面條件查詢所選學生的 `student_learning_history`
2. 顯示刪除確認資訊
3. 使用查詢取得的 `id` 精準刪除資料
4. 記錄刪除 Log
5. 刪除完成後重新整理相關學生資料

修改完成後，將本次修改內容記錄於：

`學期成績資料刪除調整0921.md`

---

# Files to Modify

主要修改：

* `frmDelStudentLearningHistory.cs`

如目前 `Program.cs` 尚未將：

```csharp
K12.Presentation.NLDPanels.Student.SelectedSource
```

傳入 `frmDelStudentLearningHistory`，則一併補上。

不要修改其他不相關功能。

---

# 1. btnDel 執行期間鎖定

## Requirement

當使用者點擊：

```text
btnDel
```

立即設定：

```csharp
btnDel.Enabled = false;
```

避免使用者連續點擊造成重複查詢或重複刪除。

所有流程完成後必須恢復：

```csharp
btnDel.Enabled = true;
```

包括：

* 查詢無資料
* 資料驗證失敗
* 使用者取消
* 刪除成功
* 發生 Exception

建議使用：

```csharp
try
{
    btnDel.Enabled = false;

    // 查詢 / 確認 / 刪除
}
catch (Exception ex)
{
    // error handling
}
finally
{
    btnDel.Enabled = true;
}
```

---

# 2. 查詢條件

由目前畫面取得：

```text
cbxSchoolYear
cbxSemester
cbxScoreType
```

另外使用 constructor 傳入的：

```text
_StudentIDs
```

作為學生系統編號清單。

查詢條件必須包含：

```text
學年度
學期
成績類別
學生系統編號
```

---

# 3. student_learning_history 資料表

資料表：

```sql
student_learning_history
```

使用欄位：

```text
id
ref_student_id
name
serial_no
school_year
semester
```

欄位意義：

```text
id
    student_learning_history 編號

ref_student_id
    學生系統編號

name
    成績名稱

serial_no
    成績類別代碼

school_year
    學年度

semester
    學期
```

---

# 4. cbxScoreType 顯示值轉換

畫面上的成績類別格式例如：

```text
學期成績(4.2)
補修成績(4.3)
轉學轉科成績(4.4)
學期成績(預檢)(41.2)
```

DB 查詢時不要直接用完整字串比對 `name`。

需要拆成：

```text
學期成績(4.2)

name = 學期成績
serial_no = 4.2
```

例如：

```text
學期成績(預檢)(41.2)

name = 學期成績(預檢)
serial_no = 41.2
```

建議新增：

```csharp
private bool TryParseScoreType(
    string displayText,
    out string scoreName,
    out string serialNo)
```

解析最後一組括號：

```text
學期成績(4.2)
→ 學期成績
→ 4.2

學期成績(預檢)(41.2)
→ 學期成績(預檢)
→ 41.2
```

---

# 5. 查詢 student_learning_history

新增獨立查詢 Method。

例如：

```csharp
private DataTable GetLearningHistoryData(
    int schoolYear,
    int semester,
    string scoreName,
    string serialNo,
    List<long> studentIDs)
```

查詢概念：

```sql
SELECT
    id,
    ref_student_id,
    school_year,
    semester,
    name,
    serial_no
FROM
    student_learning_history
WHERE
    school_year = ...
    AND semester = ...
    AND name = ...
    AND serial_no = ...
    AND ref_student_id IN (...)
ORDER BY
    ref_student_id,
    id;
```

注意：

* 學生 ID 必須來自目前選取學生。
* 不可查詢全部學生。
* 不要只使用學年度、學期查詢。
* `name` 與 `serial_no` 都必須符合。
* 查詢後保留每一筆的 `id`，後續刪除直接使用這批 `id`。

---

# 6. 學生 ID 驗證

`_StudentIDs` 為學生系統編號。

查詢前將可以正常轉成數字的 ID 整理成：

```csharp
List<long>
```

並：

```text
Distinct
```

避免：

* 重複學生
* 非數字內容
* 不安全 SQL 字串拼接

如果沒有合法學生 ID：

顯示提示後結束流程，不進行 SQL。

---

# 7. 查詢無資料

如果：

```csharp
data.Rows.Count == 0
```

顯示：

```text
查無符合條件的學習歷程資料。

學年度：xxx
學期：x
成績類別：xxx
```

不進入刪除流程。

最後仍需恢復：

```csharp
btnDel.Enabled = true;
```

---

# 8. 刪除確認畫面

查詢完成後，先統計：

```text
學年度
學期
成績類別
總學生數
總筆數
```

## 總學生數

以查詢結果內：

```text
ref_student_id
```

Distinct 後計算。

不是直接使用原本 `_StudentIDs.Count`。

因為可能部分學生沒有符合資料。

## 總筆數

使用：

```text
實際查詢到的 student_learning_history 筆數
```

---

確認畫面顯示：

```text
請確認要刪除以下學習歷程資料：

學年度：114
學期：2
成績類別：學期成績(4.2)
總學生數：35
總筆數：120

確定要刪除嗎？
```

按鈕：

```text
確定
取消
```

建議預設焦點放在：

```text
取消
```

避免使用者誤刪。

---

# 9. 使用者取消

如果使用者點：

```text
取消
```

處理：

```text
直接 return
```

不要：

* DELETE
* 寫入刪除成功 Log
* 修改資料
* 關閉主畫面

回到目前：

```text
frmDelStudentLearningHistory
```

讓使用者可以重新選擇條件。

---

# 10. 確定後使用 id 刪除

使用者按確定後：

不要重新使用：

```text
school_year
semester
name
serial_no
ref_student_id
```

作為 DELETE 條件。

必須使用「確認前查詢取得的 id」。

例如：

```sql
DELETE FROM student_learning_history
WHERE id IN (...)
RETURNING id;
```

原因：

確認畫面看到哪些資料，就刪除哪些資料。

避免：

```text
確認後 DB 又產生其他符合條件資料
```

而被一起刪除。

---

# 11. 建立刪除 Method

建議：

```csharp
private List<long> DeleteLearningHistory(
    List<long> historyIDs)
```

功能：

1. 接收查詢取得的 `id`
2. 去除重複 ID
3. 執行 DELETE
4. 使用 `RETURNING id`
5. 回傳實際刪除成功的 ID

例如：

```sql
DELETE FROM
    student_learning_history
WHERE
    id IN (...)
RETURNING
    id;
```

---

# 12. 刪除 Log

刪除完成後使用既有 FISCA Log 機制：

```csharp
FISCA.LogAgent.ApplicationLog.Log(...)
```

建議分類：

```text
刪除學習歷程資料
```

Action：

```text
刪除
```

Log 至少包含：

```text
學年度
學期
畫面成績類別
name
serial_no
總學生數
查詢總筆數
實際刪除筆數
學生系統編號
刪除的 student_learning_history id
```

例如：

```text
刪除 student_learning_history 資料

學年度：114
學期：2
成績類別：學期成績(4.2)
name：學期成績
serial_no：4.2

總學生數：35
查詢總筆數：120
實際刪除筆數：120

學生系統編號：
12345,12346,...

刪除 student_learning_history id：
100001,100002,...
```

如果：

```text
查詢總筆數 != 實際刪除筆數
```

Log 額外加入警告。

---

# 13. Exception Log

如果查詢或刪除發生 Exception：

需要：

```csharp
FISCA.LogAgent.ApplicationLog.Log(...)
```

記錄：

```text
刪除失敗
Exception.ToString()
```

畫面顯示：

```text
刪除學習歷程資料發生錯誤：
{Exception Message}
```

不要讓 Exception 直接跳出造成程式中斷。

---

# 14. 刪除完成後重新整理資料

DELETE 成功後，需要重新整理學生相關資料項目。

優先：

使用目前專案或 FISCA/K12 既有的學生資料重新整理機制。

如果已有學生資料項目對：

```text
student_learning_history
```

做 Cache / Data Item 顯示，優先呼叫該資料項目的 refresh 方法。

如果目前沒有可直接取得的特定資料項目 Refresh API，至少重新整理學生面板。

注意：

```text
重新整理失敗不應造成已完成 DELETE 被視為失敗。
```

即：

```text
DELETE 已成功
Refresh 發生問題
```

不可再執行 DELETE。

---

# 15. 完成訊息

成功完成後顯示：

```text
刪除完成。

學年度：114
學期：2
成績類別：學期成績(4.2)
總學生數：35
刪除筆數：120
```

主畫面可以保持開啟。

---

# Expected Flow

```text
btnDel Click
    ↓
btnDel.Enabled = false
    ↓
取得 SchoolYear
    ↓
取得 Semester
    ↓
取得 ScoreType
    ↓
拆解
name + serial_no
    ↓
取得目前所選 Student IDs
    ↓
查詢 student_learning_history
    ↓
有資料？
    ├─ No
    │   ↓
    │ 顯示查無資料
    │   ↓
    │ return
    │
    └─ Yes
        ↓
統計
    ├─ 總學生數
    └─ 總筆數
        ↓
顯示確認畫面
        ↓
使用者選擇
    ├─ Cancel
    │   ↓
    │ return
    │
    └─ OK
        ↓
使用查詢取得的 id
DELETE student_learning_history
        ↓
RETURNING id
        ↓
記錄 Application Log
        ↓
重新整理學生資料
        ↓
顯示完成訊息
        ↓
finally
        ↓
btnDel.Enabled = true
```

---

# Important Constraints

本次只處理：

```text
查詢 student_learning_history
確認
刪除
Log
Refresh
```

不要修改：

* 目前學年度初始化方式
* 學期初始化方式
* ScoreType 固定選項
* 權限 GUID
* 右鍵選單名稱
* 其他學生資料
* 其他學習歷程 Table
* 其他成績資料 Table
* 預檢產生邏輯
* 學習歷程上傳邏輯

---

# Safety Requirements

刪除功能必須符合：

* [ ] 沒有學生 ID 不可執行
* [ ] 沒有選學年度不可執行
* [ ] 沒有合法學期不可執行
* [ ] 沒有成績類別不可執行
* [ ] 查無資料不可執行 DELETE
* [ ] 一定先顯示確認資訊
* [ ] Cancel 不做任何資料異動
* [ ] DELETE 必須使用查詢取得的 `id`
* [ ] 不可直接用大範圍條件 DELETE
* [ ] DELETE 完成需記錄 Log
* [ ] 無論成功或失敗 btnDel 最後都必須恢復 Enabled

---

# Acceptance Criteria

完成後測試：

## Test 1：正常查詢

選：

```text
學年度
學期
成績類別
多位學生
```

確認：

```text
總學生數
總筆數
```

與 DB 查詢一致。

---

## Test 2：只有部分學生有資料

例如選 10 位學生：

```text
只有 6 位有符合的 student_learning_history
```

確認畫面：

```text
總學生數 = 6
```

不是：

```text
10
```

---

## Test 3：查無資料

確認：

```text
不出現刪除確認
不執行 DELETE
```

---

## Test 4：取消

確認畫面按：

```text
取消
```

確認 DB 完全沒有變動。

---

## Test 5：確定刪除

確認：

```text
DELETE 的 id
```

就是確認前查詢取得的 id。

---

## Test 6：刪除筆數

確認：

```text
查詢總筆數
實際刪除筆數
```

正常情況應相同。

---

## Test 7：ScoreType 解析

確認至少：

```text
學期成績(4.2)
→ name = 學期成績
→ serial_no = 4.2
```

以及：

```text
學期成績(預檢)(41.2)
→ name = 學期成績(預檢)
→ serial_no = 41.2
```

都可以正確查詢。

---

## Test 8：按鈕防重複

點擊刪除後：

```text
btnDel.Enabled = false
```

不可連續觸發兩次。

流程結束後：

```text
btnDel.Enabled = true
```

---

## Test 9：Log

確認刪除成功後：

```text
Application Log
```

可以看到：

```text
學年度
學期
成績類別
學生
刪除筆數
刪除 id
```

---

## Test 10：重新整理

刪除完成後：

確認相關學生資料可以重新載入。

---

# Modification Record

完成修改後建立：

`學期成績資料刪除調整0921.md`

內容至少記錄：

1. 修改日期
2. 修改檔案
3. student_learning_history 查詢條件
4. ScoreType 如何轉換成 name / serial_no
5. 學生 ID 查詢方式
6. 確認畫面統計方式
7. DELETE 使用 id 的設計
8. Log 記錄內容
9. 資料重新整理方式
10. Exception 處理
11. 測試案例與結果
12. 是否影響其他既有功能

請記錄實際修改內容，不要只寫「完成」。
