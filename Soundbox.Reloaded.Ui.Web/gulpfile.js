/// <binding BeforeBuild='typescript-compile' AfterBuild='less-compile' />
var gulp = require('gulp');
var shell = require('gulp-shell');

gulp.task('typescript-compile',
    shell.task(['tsc --project ./'])
);

gulp.task('typescript-watch',
    shell.task(['tsc --project ./ --watch'])
);

gulp.task('less-compile',
    shell.task(['lessc "Styles/icons.less" "Styles/icons.css"',
                'lessc "Styles/app.less" "Styles/app.css"'])
);