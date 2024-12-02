import Foundation

let inputPath = "D:\\wd\\advent-of-code-2024\\day1\\input.txt"

func part1() {
    var distance: Int = 0

    do {
        let fileContent = try String(contentsOfFile: inputPath, encoding: .utf8)
        let lines = fileContent.components(separatedBy: .newlines).filter { !$0.isEmpty }
        var bag1: [Int] = []
        var bag2: [Int] = []
        for line in lines {
            let parts = line.split(whereSeparator: \.isWhitespace).map { $0.trimmingCharacters(in: .whitespaces)}

            bag1.append(Int(parts[0])!)
            bag2.append(Int(parts[1])!)
        }

        bag1.sort()
        bag2.sort()

        for (num1, num2) in zip(bag1, bag2) {
            distance += abs(num1 - num2)
        }
    } catch {
        print("Error reading file: \(error.localizedDescription)")
    }

    print("Total distance: \(distance)")
}

func part2() {
    var similarity: Int = 0

    do {
        let fileContent = try String(contentsOfFile: inputPath, encoding: .utf8)
        let lines = fileContent.components(separatedBy: .newlines).filter { !$0.isEmpty }
        var bag1: [Int] = []
        var freqDict: [Int: Int] = [:]
        for line in lines {
            let parts = line.split(whereSeparator: \.isWhitespace).map { $0.trimmingCharacters(in: .whitespaces)}
            
            bag1.append(Int(parts[0])!)
            
            let key = Int(parts[1])!
            freqDict[key] = (freqDict[key] ?? 0) + 1
        }

        for num in bag1 {
            similarity += num * (freqDict[num] ?? 0)
        }
    } catch {
        print("Error reading file: \(error.localizedDescription)")
    }

    print("Total similarity: \(similarity)")
}

part1()
part2()