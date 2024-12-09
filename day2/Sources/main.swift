import Foundation

let inputPath = "input.txt"

// If I have seen too many errors, I fail
// if I don't have any more values, I pass
// if I have a current value and no previous value, I can either use or omit my value
// if I have a current value and a previous value and it passes, I can either use or omit my value
// if I have a current value and a previous value and it fails, I can only omit my value

func isArraySafeRec(_ array: [Int], possiblePrevious: Int? = nil, possibleIsIncreasing: Bool? = nil, maxIgnorableErrors: Int = 0) -> Bool
{
    if (maxIgnorableErrors < 0) {
        return false
    }

    if (array.count == 0) {
        return true
    }

    let current = array[0]

    if let previous = possiblePrevious {
        let difference = current - previous

        if ((abs(difference) < 1 || abs(difference) > 3) ||
            (possibleIsIncreasing != nil && ((difference > 0 && !possibleIsIncreasing!) || (difference < 0 && possibleIsIncreasing!))))
        {
            return isArraySafeRec(Array(array.dropFirst()), possiblePrevious: possiblePrevious, possibleIsIncreasing: possibleIsIncreasing, maxIgnorableErrors: maxIgnorableErrors - 1)
        } else {
            return isArraySafeRec(Array(array.dropFirst()), possiblePrevious: current, possibleIsIncreasing: difference > 0 ? true : false, maxIgnorableErrors: maxIgnorableErrors) ||
                   isArraySafeRec(Array(array.dropFirst()), possiblePrevious: possiblePrevious, possibleIsIncreasing: possibleIsIncreasing, maxIgnorableErrors: maxIgnorableErrors - 1)
        }
    } else {
        return isArraySafeRec(Array(array.dropFirst()), possiblePrevious: current, possibleIsIncreasing: nil, maxIgnorableErrors: maxIgnorableErrors) ||
               isArraySafeRec(Array(array.dropFirst()), possiblePrevious: nil, possibleIsIncreasing: nil, maxIgnorableErrors: maxIgnorableErrors - 1)
    }
}

func part1() {
    var safeReports: Int = 0

    do {
        let fileContent = try String(contentsOfFile: inputPath, encoding: .utf8)
        let lines = fileContent.components(separatedBy: .newlines).filter { !$0.isEmpty }

        for line in lines {
            let parts = line.split(whereSeparator: \.isWhitespace).compactMap { Int($0.trimmingCharacters(in: .whitespaces)) }

            if (isArraySafeRec(parts))
            {
                safeReports += 1
            }
        }

    } catch {
        print("Error reading file: \(error.localizedDescription)")
    }

    print("part 2: \(safeReports)")
}

func part2() {
    var safeReports: Int = 0

    do {
        let fileContent = try String(contentsOfFile: inputPath, encoding: .utf8)
        let lines = fileContent.components(separatedBy: .newlines).filter { !$0.isEmpty }

        for line in lines {
            let parts = line.split(whereSeparator: \.isWhitespace).compactMap { Int($0.trimmingCharacters(in: .whitespaces)) }

            if (isArraySafeRec(parts, maxIgnorableErrors: 1))
            {
                safeReports += 1
            }
        }

    } catch {
        print("Error reading file: \(error.localizedDescription)")
    }

    print("part 2: \(safeReports)")
}

part1()
part2()