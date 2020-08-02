
exports.up = function(knex) {
    return knex.schema.alterTable('imtb_house', function(t) {
        t.string('Guid', 32);
    });
};

exports.down = function(knex) {
  return knex.schema.alterTable('imtb_house', function(t) {
    t.dropColumn('Guid');
  });
};
