
exports.up = function(knex) {
    return knex.schema.alterTable('imtb_account', function(t) {
        t.float('LastX', 14, 4).notNull().defaultTo(0);
        t.float('LastY', 14, 4).notNull().defaultTo(0);
        t.float('LastZ', 14, 4).notNull().defaultTo(0);
        t.integer('LastHouseInside').nullable().defaultTo(null);
    });
};

exports.down = function(knex) {
  return knex.schema.alterTable('imtb_account', function(t) {
    t.dropColumn('LastX');
    t.dropColumn('LastY');
    t.dropColumn('LastZ');
    t.dropColumn('LastHouseInside');
  });
};
