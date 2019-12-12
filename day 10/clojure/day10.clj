(require '[clojure.string :as str])

(defn delta-x [fro to]
    (- (:x to) (:x fro)))

(defn delta-y [fro to]
    (- (:y fro) (:y to)))

(defn get-angle [fro to]
    (let [angle (Math/toDegrees (Math/atan2 (delta-x fro to) (delta-y fro to)))]
        (if (< angle 0) (+ angle 360) angle)))

(def example (slurp "<insert-path>\\input.txt"))
(def mapped (str/split example #"\r\n"))

(defn asteroid-locations
    ([map]
        (asteroid-locations map 0 []))

    ([map y found-asteroids]
        (if (empty? map)
            found-asteroids
            (let [next-asteroids (asteroid-locations (first map) 0 y [])]
                (asteroid-locations (rest map) (inc y) (conj found-asteroids next-asteroids)))))

    ([x-axis x y found-asteroids]
        (let [another (str/index-of x-axis "#" x)]
            (if (nil? another)
                found-asteroids
                (recur x-axis (inc another) y (conj found-asteroids [another y]))))))

(defn re-coordinate
    ([input]
        (re-coordinate input []))
    ([input output]
        (if (empty? input) 
            output
            (recur (nthrest input 2) (conj output [(first input) (second input)])))))

(def asteroids (re-coordinate (flatten (asteroid-locations mapped))))

(defn except-self [asteroids asteroid]
    (let [[X Y] asteroid]
        (filter #(or (not= (first %) X) (not= (second %) Y)) asteroids)))

(defn get-asteroid-angles 
    ([asteroid asteroids]
        (get-asteroid-angles asteroid (except-self asteroids asteroid) []))

    ([asteroid asteroids angles]
        (if (empty? asteroids)
            (distinct angles)
            (let [
                    [x1 y1] asteroid
                    [x2 y2] (first asteroids)
                    angle (get-angle {:x x1 :y y1} {:x x2 :y y2})
                ]
                (recur asteroid (rest asteroids) (conj angles angle))))))

(defn visible-asteroids
    ([asteroids]
        (visible-asteroids asteroids 0 {}))

    ([asteroids id sighted-asteroids]
        (if (= id (count asteroids))
            sighted-asteroids
            (let [
                    current-asteroid (nth asteroids id)
                    newly-sighted (count (get-asteroid-angles current-asteroid asteroids))
                ]
                (recur asteroids (inc id) (assoc sighted-asteroids current-asteroid newly-sighted))))))

(def sighted (visible-asteroids asteroids))
(def asteroid-with-best-view (key (apply max-key val sighted)))
(def part-one (get sighted asteroid-with-best-view))


(defn same-asteroid? [asteroid-one asteroid-two]
    (and 
        (= (first asteroid-one) (first asteroid-two)) 
        (= (second asteroid-one) (second asteroid-two))))

(defn get-id 
    ([asteroids asteroid]
        (get-id asteroids asteroid 0))
    
    ([asteroids asteroid id]
        (if (same-asteroid? (nth asteroids id) asteroid)
            id
            (recur asteroids asteroid (inc id)))))

(defn asteroid-and-angle 
    ([asteroids asteroid]
        (asteroid-and-angle (except-self asteroids asteroid) asteroid {}))

    ([asteroids asteroid asteroid-angles]
        (if (empty? asteroids)
            asteroid-angles
            (let [
                    [x1 y1] asteroid
                    [x2 y2] (first asteroids)
                    angle (get-angle {:x x1 :y y1} {:x x2 :y y2})
                ]
                (recur (rest asteroids) asteroid (assoc asteroid-angles (first asteroids) angle))))))

(def angles-to-asteroids (asteroid-and-angle asteroids asteroid-with-best-view))

(defn keyed-by-angle
    ([angles]
        (keyed-by-angle (distinct (vals angles)) {}))
    ([angles keyed]
        (if (empty? angles)
            keyed
            (recur (rest angles) (assoc keyed (first angles) [])))))

;; (def angle-keys (keyed-by-angle angle-for-asteroid))

(defn asteroids-by-angle
    ([angle-for-asteroids]
        (asteroids-by-angle angle-for-asteroids (keyed-by-angle angle-for-asteroid)))

    ([angle-for-asteroids by-angle]
        (if (empty? angle-for-asteroids)
            by-angle
            (let [
                    current-asteroid (first angle-for-asteroids)
                    asteroid (key current-asteroid)
                    angle (val current-asteroid)
                    asteroids-at-angle (get by-angle angle)
                ]
            (recur (rest angle-for-asteroids) (assoc by-angle angle (conj asteroids-at-angle asteroid)))))))

(def asteroids-map ;; { angle [asteroid1 asteroid2] }
    (into (sorted-map) (asteroids-by-angle angles-to-asteroids)))

(defn get-distance [asteroid1 asteroid2]
    (let [  a (Math/abs (- (first asteroid1) (first asteroid2)))
            b (Math/abs (- (second asteroid1) (second asteroid2))) ]
        [asteroid2 (Math/sqrt (+ (Math/pow a 2) (Math/pow b 2)))]))

;; (get-distance asteroid-with-best-view [0 0])

(defn closest-asteroid [asteroids asteroid]
    (let [
            distances (map (partial get-distance asteroid) asteroids)
            mapped-distances (into (sorted-map) distances)
        ]
        (key (apply min-key val mapped-distances))))

;; (closest-asteroid [[0 0] [10 15]] asteroid-with-best-view)

(defn first-asteroids-exceeding-angle [asteroids angle-to-exceed]
    (let [current-angle (first (first asteroids))]
        (if (> current-angle angle-to-exceed)
            (first asteroids)
            (recur (rest asteroids) angle-to-exceed))))

;; (first (first-asteroids-exceeding-angle { 0.0 [[1 1] [2 2]] 1.5 [[3 3]] } 0.0 ))

(defn sweep-laser
    ([asteroids] ;; { angle [asteroid1 asteroid2] }
        (sweep-laser asteroids -1 {}))

    ([asteroids angle-to-exceed lasered]
        (if (= 200 (count lasered))
            lasered
            (let [
                    angle->asteroids (first-asteroids-exceeding-angle asteroids angle-to-exceed)
                    closest-at-this-angle (closest-asteroid (second angle->asteroids) asteroid-with-best-view)
                    removed-closest (except-self (second angle->asteroids) closest-at-this-angle)
                    asteroids-except-removed (assoc asteroids-map (first angle->asteroids) removed-closest)
                ]
                (recur asteroids-except-removed (first angle->asteroids) (assoc lasered (count lasered) closest-at-this-angle))))))

(def the-200th-asteroid-lasered (get (sweep-laser asteroids-map) 199))

(def part-two (+ 4 (* 100 (first the-200th-asteroid-lasered))))
