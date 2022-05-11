/// <binding BeforeBuild='default' Clean='clean' />
'use strict';

var gulp = require('gulp'),
    $ = require('gulp-load-plugins')(),
    del = require('del'),
    saveLicense = require('uglify-save-license'),
    cssnano = require('cssnano');

const debug = require('gulp-debug');

gulp.task('default', () =>
    gulp.src('foo.js')
        .pipe(debug({ title: 'unicorn:' }))
        .pipe(gulp.dest('dist'))
);

var paths = {
    webroot: './wwwroot/',
    sources: './wwwroot/master/'
};

// Sources
paths.js = [
    paths.sources + 'js/modules/common/wrapper.js',
    paths.sources + 'js/app.init.js',
    paths.sources + 'js/modules/**/*.js',
    paths.sources + 'js/custom/**/*.js'
];

paths.css = paths.webroot + 'css/**/*.css';
paths.sass = paths.sources + 'sass/app.scss';
paths.sassThemes = paths.sources + 'sass/themes/*.scss';
paths.sassBs = paths.sources + 'sass/bootstrap.scss';
paths.sassWatch = paths.sources + 'sass/**/*.scss';

// Dests
paths.concatJsDest = paths.webroot + 'js';
paths.concatCssDest = paths.webroot + 'css';

// VENDOR CONFIG
var vendor = {
    source: require('./vendor.json'),
    dest: paths.webroot + 'vendor'
};

var cssnanoOpts = {
    safe: true,
    discardUnused: false, // no remove @font-face
    reduceIdents: false, // no change on @keyframes names
    zindex: false // no change z-index
}

var vendorUglifyOpts = {
    output: {
        comments: false,
    },
    mangle: {
        reserved: ['$super'] // rickshaw requires this
    }
};


gulp.task('vendor', function() {

    return gulp.src(vendor.source, {
            base: 'node_modules'
        })
        .pipe(gulp.dest(vendor.dest));

});

gulp.task('min:vendor', function() {

    var jsFilter = $.filter('**/*.js', {
        restore: true
    });
    var cssFilter = $.filter('**/*.css', {
        restore: true
    });

    return gulp.src(vendor.source, {
            base: 'node_modules'
        })
        .pipe(jsFilter)
        .pipe($.uglify(vendorUglifyOpts))
        .on('error', handleError)
        .pipe(jsFilter.restore)
        .pipe(cssFilter)
        .pipe($.postcss([cssnano(cssnanoOpts)]))
        .pipe(cssFilter.restore)
        .pipe(gulp.dest(vendor.dest));

});

gulp.task('clean:js', function(cb) {
    return del([paths.concatJsDest], { force: true })
});

gulp.task('clean:css', function(cb) {
    return del([paths.concatCssDest], { force: true })
});

gulp.task('clean:vendor', function(cb) {
    return del(vendor.dest, { force: true });
});

gulp.task('clean', gulp.series('clean:js', 'clean:css'));


gulp.task('js', function() {
    return gulp.src(paths.js)
        .pipe($.concat('app.js'))
        .pipe($.babel())
        .on('error', handleError)
        .pipe(gulp.dest(paths.concatJsDest));
});

gulp.task('min:js', function() {
    return gulp.src(paths.js)
        .pipe($.concat('app.js'))
        .pipe($.babel())
        .on('error', handleError)
        .pipe($.uglify({
            output: {
                comments: saveLicense
            }
        }))
        .pipe(gulp.dest(paths.concatJsDest));
});

gulp.task('sass:app', function() {
    return gulp.src(paths.sass)
        .pipe($.sass())
        .on('error', $.sass.logError)
        .pipe(gulp.dest(paths.concatCssDest));
});

gulp.task('sass:bs', function() {
    return gulp.src(paths.sassBs)
        .pipe($.sass())
        .on('error', $.sass.logError)
        .pipe(gulp.dest(paths.concatCssDest));
});

gulp.task('sass:themes', function() {
    return gulp.src(paths.sassThemes)
        .pipe($.sass())
        .on('error', $.sass.logError)
        .pipe(gulp.dest(paths.concatCssDest));
});

gulp.task('sass:watch', function() {
    gulp.watch(paths.sassWatch, gulp.task('sass'));
});

gulp.task('sass', gulp.parallel('sass:bs', 'sass:app', 'sass:themes'));

gulp.task('rtl', function() {
    return gulp.src(paths.webroot + 'css/**/{app,bootstrap}.css')
        .pipe($.rtlcss())
        .pipe($.rename(function(path) {
            path.basename += '-rtl';
            return path;
        }))
        .pipe(gulp.dest(paths.concatCssDest));
});

gulp.task('min:css', function() {
    return gulp.src(paths.css)
        .pipe($.postcss([cssnano(cssnanoOpts)]))
        .pipe(gulp.dest(paths.concatCssDest));
});

gulp.task('min', gulp.series('min:vendor', 'min:css', 'min:js'));
gulp.task('default', gulp.series('vendor', 'js', 'sass', 'rtl'));

// Task names for each ConfigurationName
// see file Angle.csproj
gulp.task('Release', gulp.task('min'));
gulp.task('Debug', gulp.task('default'));


function handleError(err) {
    console.log(err.toString());
    this.emit('end');
}