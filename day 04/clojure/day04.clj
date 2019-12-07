(defn top [characters]
    (subs characters 0 1))

(defn remaining [characters]
    (subs characters 1))

(defn parse-int [number-string]
    (try (Integer/parseInt number-string)
      (catch Exception e nil)))

(defn right-lenth? [password]
    (= 6 (count password)))

(defn within-range? [min max number]
    (and 
        (>= (parse-int number) (parse-int min)) 
        (<= (parse-int number) (parse-int max))))

(defn same-adjacent-chars? [head tail]
    (if (or (empty? head) (empty? tail))
        false
        (if (= head (top tail))
            true
            (same-adjacent-chars? (top tail) (remaining tail)))))

(defn same-adjacent? [password]
    (same-adjacent-chars? (top password) (remaining password)))

(defn not-decreases? [head tail]
    (if (empty? tail)
        true
        (<= (parse-int head) (parse-int (top tail)))))

(defn never-decreases? [numbers]
    (if (empty? numbers)
        true
        (if (not-decreases? (top numbers) (remaining numbers))
            (never-decreases? (remaining numbers))   
            false)))

(defn verify [checks input]
    (if (empty? checks)
        true
        (and ((first checks) input) (verify (rest checks) input))))

(defn partial-verify[] 
    (partial verify [
        right-lenth?
        (partial within-range? "240920" "789857")
        same-adjacent?
        never-decreases?]))

(defn show-valid [passwords valid-passwords]
    (if (empty? passwords)
        valid-passwords
        (let [password (first passwords)]
        (if ((partial-verify) password)
            (recur (rest passwords) (conj valid-passwords password))
            (recur (rest passwords) valid-passwords)))))

(count (show-valid (map str (range 200000 800000)) []))
