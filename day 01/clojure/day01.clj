(require '[clojure.string :as str])

(defn get-fuel-required [mass]    
    (int (- (/ mass 3) 2)))

(assert (= 2 (get-fuel-required 12)))
(assert (= 2 (get-fuel-required 14)))
(assert (= 654 (get-fuel-required 1969)))
(assert (= 33583 (get-fuel-required 100756)))

(def file-content (slurp "input.txt"))
(def input (str/split file-content #"\n"))
(def masses (map read-string input))
(def part-one-fuel-needs (map get-fuel-required masses))
(def part-one-answer (reduce + part-one-fuel-needs))
(assert (= 3269199 part-one-answer))

(defn get-total-fuel-required [mass]
    (comment (println mass))
    (def fuel-need (get-fuel-required mass))
    (comment (println fuel-need))
    (if (<= fuel-need 0) 0 (+ fuel-need (get-total-fuel-required fuel-need))))

(assert (= 2 (get-total-fuel-required 12)))
(assert (= 2 (get-total-fuel-required 14)))
(assert (= 966 (get-total-fuel-required 1969)))
(assert (= 50346 (get-total-fuel-required 100756)))

(def part-two-fuel-needs (map get-total-fuel-required masses))
(def part-two-answer (reduce + part-two-fuel-needs))
(assert (= 4900909 part-two-answer))
