/// <binding BeforeBuild='build-dev' />
import gulp from "gulp";
import autoprefixer from "gulp-autoprefixer";
import rename from "gulp-rename";
import { deleteSync } from "del";
import cleanCSS from "gulp-clean-css";

import * as dartSass from "sass";
import gulpSass from "gulp-sass";
const sass = gulpSass(dartSass);

/* SCSS to CSS ---------------------------------------------------------------*/
const scss = async () => {
  deleteSync([`wwwroot/style.css`]);
  return new Promise((resolve, reject) => {
    gulp
      .src(`Styles/style.scss`)
      .pipe(sass.sync().on("error", sass.logError))
      .pipe(
        autoprefixer({
          overrideBrowserslist: ["last 2 versions"],
        })
      )
      .pipe(gulp.dest("wwwroot/css"))
      .on("end", resolve)
      .on("error", reject);
  });
};

gulp.task("clean:css", (cb) => {
  deleteSync([`${path.css.dest}`]);
  cb();
});

gulp.task("min:css", async () => {
  return gulp
    .src(`${path.css.src}/**/*.css`, { allowEmpty: true })
    .pipe(rename({ suffix: ".min" }))
    .pipe(
      cleanCSS({
        inline: ["none"],
      })
    )
    .pipe(gulp.dest(path.css.dest));
});

/* watch---------------------------------------------------------------------*/
gulp.task("watch", async () => {
  gulp.watch(`Styles/**/**/*.scss`, gulp.series([scss]));
});

/* build ---------------------------------------------------------------------*/
gulp.task("clean", gulp.series(["clean:css"]));
gulp.task("minStyles", gulp.series(["clean:css", "min:css"]));
gulp.task("style", gulp.series([scss, "minStyles"]));

gulp.task("build-dev", gulp.series([scss]));
gulp.task("build", gulp.series(["style"]));
